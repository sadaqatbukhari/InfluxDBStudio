using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CymaticLabs.InfluxDB.Data;
using Newtonsoft.Json;

namespace CymaticLabs.InfluxDB.Studio
{
    /// <summary>
    /// Application settings stored in a version-independent per-user location.
    /// </summary>
    public class AppSettings
    {
        public const string TimeFormat12Hour = "hh:mm:ss tt";
        public const string TimeFormat24Hour = "HH:mm:ss";
        public const string DateFormatDay = "d/MM/yyyy";
        public const string DateFormatMonth = "M/dd/yyyy";

        private const string SettingsDirectoryName = "InfluxDB Studio";
        private const string SettingsFileName = "settings.json";

        private readonly string settingsFilePath;
        private readonly IEnumerable<string> legacySearchRoots;
        private bool allowUntrustedSsl;
        private string timeFormat;
        private string dateFormat;

        public string Version { get; private set; }

        /// <summary>
        /// Gets the fixed settings file path. This path does not contain the application version,
        /// so MSI and bundle upgrades cannot change or remove it.
        /// </summary>
        public string SettingsFilePath => settingsFilePath;

        public string TimeFormat
        {
            get { return timeFormat; }
            set
            {
                if (timeFormat != value)
                {
                    timeFormat = value;
                    SaveAll();
                }
            }
        }

        public string DateFormat
        {
            get { return dateFormat; }
            set
            {
                if (dateFormat != value)
                {
                    dateFormat = value;
                    SaveAll();
                }
            }
        }

        public bool AllowUntrustedSsl
        {
            get { return allowUntrustedSsl; }
            set
            {
                if (allowUntrustedSsl != value)
                {
                    allowUntrustedSsl = value;
                    SaveAll();
                }
            }
        }

        public List<InfluxDbConnection> Connections { get; set; }

        public AppSettings()
            : this(GetDefaultSettingsFilePath(), GetDefaultLegacySearchRoots())
        {
        }

        /// <summary>
        /// Allows the storage path and legacy locations to be supplied for verification without
        /// reading or changing the real user's profile.
        /// </summary>
        internal AppSettings(string filePath, IEnumerable<string> searchRoots)
        {
            settingsFilePath = filePath;
            legacySearchRoots = searchRoots ?? Enumerable.Empty<string>();
            timeFormat = TimeFormat12Hour;
            dateFormat = DateFormatMonth;
            Connections = new List<InfluxDbConnection>();
            Version = GetType().Assembly.GetName().Version?.ToString() ?? "0.0.0.0";
        }

        public void LoadAll()
        {
            PersistedAppSettings settings = null;

            if (File.Exists(settingsFilePath))
            {
                settings = ReadSettingsFile(settingsFilePath);
            }
            else
            {
                settings = ReadLegacySettings();
            }

            if (settings == null)
            {
                return;
            }

            timeFormat = string.IsNullOrWhiteSpace(settings.TimeFormat)
                ? TimeFormat12Hour
                : settings.TimeFormat;
            dateFormat = string.IsNullOrWhiteSpace(settings.DateFormat)
                ? DateFormatMonth
                : settings.DateFormat;
            allowUntrustedSsl = settings.AllowUntrustedSsl;
            Connections = settings.Connections ?? new List<InfluxDbConnection>();

            // A legacy import is immediately written to the stable path. Future versions then
            // read the same file and never depend on .NET's version-scoped user.config folder.
            if (!File.Exists(settingsFilePath))
            {
                SaveAll();
            }
        }

        public void SaveAll()
        {
            try
            {
                var directory = Path.GetDirectoryName(settingsFilePath);
                if (string.IsNullOrWhiteSpace(directory))
                {
                    throw new InvalidOperationException("The settings file must have a directory.");
                }

                Directory.CreateDirectory(directory);
                var settings = new PersistedAppSettings
                {
                    Version = Version,
                    TimeFormat = TimeFormat,
                    DateFormat = DateFormat,
                    AllowUntrustedSsl = AllowUntrustedSsl,
                    Connections = Connections ?? new List<InfluxDbConnection>()
                };

                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                var temporaryPath = settingsFilePath + ".tmp";
                File.WriteAllText(temporaryPath, json);
                File.Move(temporaryPath, settingsFilePath, true);
            }
            catch (Exception ex)
            {
                AppForm.DisplayException(ex);
            }
        }

        public void LoadConnections()
        {
            LoadAll();
        }

        public void SaveConnections()
        {
            SaveAll();
        }

        private PersistedAppSettings ReadSettingsFile(string path)
        {
            try
            {
                var json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<PersistedAppSettings>(json);
            }
            catch (Exception ex)
            {
                AppForm.DisplayException(ex);
                return null;
            }
        }

        private PersistedAppSettings ReadLegacySettings()
        {
            var candidates = new List<LegacySettingsCandidate>();

            // Include settings already found by .NET for the current executable identity.
            var current = CreateLegacySettings(
                Properties.Settings.Default.ConnectionsJson,
                Properties.Settings.Default.TimeFormat,
                Properties.Settings.Default.DateFormat,
                Properties.Settings.Default.AllowUntrustedSsl);
            if (current != null)
            {
                candidates.Add(new LegacySettingsCandidate(current, DateTime.MaxValue));
            }

            foreach (var root in legacySearchRoots.Where(Directory.Exists).Distinct(StringComparer.OrdinalIgnoreCase))
            {
                try
                {
                    foreach (var path in Directory.EnumerateFiles(root, "user.config", SearchOption.AllDirectories))
                    {
                        var settings = ReadLegacyUserConfig(path);
                        if (settings != null)
                        {
                            candidates.Add(new LegacySettingsCandidate(settings, File.GetLastWriteTimeUtc(path)));
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // A protected sibling directory should not prevent migration from other roots.
                }
                catch (IOException)
                {
                    // A stale or locked legacy directory can safely be skipped.
                }
            }

            return candidates
                .OrderByDescending(candidate => candidate.Settings.Connections?.Count > 0)
                .ThenByDescending(candidate => candidate.LastWriteTimeUtc)
                .Select(candidate => candidate.Settings)
                .FirstOrDefault();
        }

        private static PersistedAppSettings ReadLegacyUserConfig(string path)
        {
            try
            {
                var document = XDocument.Load(path);
                var values = document
                    .Descendants()
                    .Where(element => element.Name.LocalName == "setting")
                    .Where(element => element.Attribute("name") != null)
                    .GroupBy(element => (string)element.Attribute("name"), StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Last().Elements().FirstOrDefault(element => element.Name.LocalName == "value")?.Value,
                        StringComparer.OrdinalIgnoreCase);

                values.TryGetValue("ConnectionsJson", out var connectionsJson);
                values.TryGetValue("TimeFormat", out var legacyTimeFormat);
                values.TryGetValue("DateFormat", out var legacyDateFormat);
                values.TryGetValue("AllowUntrustedSsl", out var legacyAllowUntrustedSsl);
                bool.TryParse(legacyAllowUntrustedSsl, out var parsedAllowUntrustedSsl);

                return CreateLegacySettings(
                    connectionsJson,
                    legacyTimeFormat,
                    legacyDateFormat,
                    parsedAllowUntrustedSsl);
            }
            catch
            {
                // Invalid or unrelated user.config files are ignored during best-effort migration.
                return null;
            }
        }

        private static PersistedAppSettings CreateLegacySettings(
            string connectionsJson,
            string legacyTimeFormat,
            string legacyDateFormat,
            bool legacyAllowUntrustedSsl)
        {
            List<InfluxDbConnection> connections = null;
            if (!string.IsNullOrWhiteSpace(connectionsJson))
            {
                try
                {
                    connections = JsonConvert.DeserializeObject<List<InfluxDbConnection>>(connectionsJson);
                }
                catch (JsonException)
                {
                    return null;
                }
            }

            var hasSettings = connections?.Count > 0
                || (!string.IsNullOrWhiteSpace(legacyTimeFormat) && legacyTimeFormat != TimeFormat12Hour)
                || (!string.IsNullOrWhiteSpace(legacyDateFormat) && legacyDateFormat != DateFormatMonth)
                || legacyAllowUntrustedSsl;

            if (!hasSettings)
            {
                return null;
            }

            return new PersistedAppSettings
            {
                TimeFormat = legacyTimeFormat,
                DateFormat = legacyDateFormat,
                AllowUntrustedSsl = legacyAllowUntrustedSsl,
                Connections = connections ?? new List<InfluxDbConnection>()
            };
        }

        private static string GetDefaultSettingsFilePath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CymaticLabs",
                SettingsDirectoryName,
                SettingsFileName);
        }

        private static IEnumerable<string> GetDefaultLegacySearchRoots()
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var directoryNames = new[]
            {
                "CymaticLabs",
                "Cymatic_Labs",
                "InfluxDBStudio",
                "InfluxDB_Studio",
                "InfluxStudio_Core"
            };

            return new[] { localAppData, appData }
                .Where(path => !string.IsNullOrWhiteSpace(path))
                .SelectMany(path => directoryNames.Select(name => Path.Combine(path, name)));
        }

        private sealed class PersistedAppSettings
        {
            public string Version { get; set; }
            public string TimeFormat { get; set; }
            public string DateFormat { get; set; }
            public bool AllowUntrustedSsl { get; set; }
            public List<InfluxDbConnection> Connections { get; set; }
        }

        private sealed class LegacySettingsCandidate
        {
            public LegacySettingsCandidate(PersistedAppSettings settings, DateTime lastWriteTimeUtc)
            {
                Settings = settings;
                LastWriteTimeUtc = lastWriteTimeUtc;
            }

            public PersistedAppSettings Settings { get; }
            public DateTime LastWriteTimeUtc { get; }
        }
    }
}
