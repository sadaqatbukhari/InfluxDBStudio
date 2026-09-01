using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using CymaticLabs.InfluxDB.Data;

namespace CymaticLabs.InfluxDB.Studio.Controls
{
    internal sealed class InfluxQueryIntellisense
    {
        private const string InfluxQlResourceName =
            "CymaticLabs.InfluxDB.Studio.Resources.InfluxQL.xml";

        private static readonly Lazy<Completion[]> InfluxQlItems =
            new Lazy<Completion[]>(LoadInfluxQlItems);

        private static readonly Completion[] SqlItems = CreateItems("SQL keyword", new[]
        {
            "SELECT", "FROM", "WHERE", "GROUP BY", "ORDER BY", "LIMIT", "OFFSET", "AS",
            "AND", "OR", "NOT", "IN", "IS NULL", "IS NOT NULL", "ASC", "DESC", "DISTINCT",
            "COUNT()", "AVG()", "SUM()", "MIN()", "MAX()", "DATE_BIN()", "NOW()"
        }).ToArray();

        private readonly HashSet<string> measurements = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Completion[]> schemaByMeasurement = new(StringComparer.OrdinalIgnoreCase);

        public IEnumerable<Completion> GetItems(InfluxDbClient client, string queryText)
        {
            var standardItems = client?.Connection.ServerVersion == InfluxDbServerVersion.InfluxDb3
                ? SqlItems
                : InfluxQlItems.Value;
            var measurementItems = measurements.Select(name => new Completion(name, "Measurement/table"));
            var measurement = FindMeasurement(queryText);
            var schemaItems = measurement != null && schemaByMeasurement.TryGetValue(measurement, out var schema)
                ? schema
                : Array.Empty<Completion>();

            return standardItems.Concat(measurementItems).Concat(schemaItems)
                .GroupBy(item => item.Text, StringComparer.OrdinalIgnoreCase)
                .Select(group => group.First())
                .OrderBy(item => item.Text, StringComparer.OrdinalIgnoreCase);
        }

        public async Task LoadMeasurementsAsync(InfluxDbClient client, string database)
        {
            if (client == null || string.IsNullOrWhiteSpace(database)) return;
            var names = await client.GetMeasurementNamesAsync(database);
            foreach (var name in names ?? Enumerable.Empty<string>())
                if (!string.IsNullOrWhiteSpace(name)) measurements.Add(name);
        }

        public async Task LoadContextSchemaAsync(InfluxDbClient client, string database, string queryText)
        {
            var measurement = FindMeasurement(queryText);
            if (client == null || string.IsNullOrWhiteSpace(database) || measurement == null
                || schemaByMeasurement.ContainsKey(measurement)) return;

            var fieldsTask = client.GetFieldKeysAsync(database, measurement);
            var tagsTask = client.GetTagKeysAsync(database, measurement);
            await Task.WhenAll(fieldsTask, tagsTask);
            var fields = (await fieldsTask ?? Enumerable.Empty<InfluxDbFieldKey>())
                .Select(field => new Completion(field.Name, "Field"));
            var tags = (await tagsTask ?? Enumerable.Empty<string>())
                .Select(tag => new Completion(tag, "Tag"));
            schemaByMeasurement[measurement] = fields.Concat(tags).ToArray();
        }

        private static string FindMeasurement(string queryText)
        {
            if (string.IsNullOrWhiteSpace(queryText)) return null;
            var matches = Regex.Matches(queryText,
                "\\bFROM\\s+(?:\"(?<quoted>[^\"]+)\"|(?<plain>[A-Za-z_][\\w.:-]*))",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            if (matches.Count == 0) return null;
            var match = matches[matches.Count - 1];
            return match.Groups["quoted"].Success ? match.Groups["quoted"].Value : match.Groups["plain"].Value;
        }

        private static IEnumerable<Completion> CreateItems(string description, IEnumerable<string> values) =>
            values.Select(value => new Completion(value, description));

        private static Completion[] LoadInfluxQlItems()
        {
            using (var stream = typeof(InfluxQueryIntellisense).Assembly
                .GetManifestResourceStream(InfluxQlResourceName))
            {
                if (stream == null)
                    throw new InvalidOperationException(
                        "The embedded InfluxQL IntelliSense configuration could not be found.");

                var document = XDocument.Load(stream);
                return document.Descendants()
                    .Where(element => element.Name.LocalName == "Lexem")
                    .Select(element => new
                    {
                        Text = (string)element.Attribute("BeginBlock"),
                        Format = (string)element.Attribute("FormatName")
                    })
                    .Where(item => !string.IsNullOrWhiteSpace(item.Text))
                    .Select(item => new Completion(
                        string.Equals(item.Format, "Function", StringComparison.OrdinalIgnoreCase)
                            ? item.Text + "()"
                            : item.Text,
                        string.Equals(item.Format, "Function", StringComparison.OrdinalIgnoreCase)
                            ? "Function"
                            : "Keyword"))
                    .GroupBy(item => item.Text, StringComparer.OrdinalIgnoreCase)
                    .Select(group => group.First())
                    .ToArray();
            }
        }

        internal sealed class Completion
        {
            public Completion(string text, string description) { Text = text; Description = description; }
            public string Text { get; }
            public string Description { get; }
        }
    }
}
