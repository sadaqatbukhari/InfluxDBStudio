using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CymaticLabs.InfluxDB.Data;
using Syncfusion.Windows.Forms.Edit.Enums;
using Syncfusion.Windows.Forms.Edit.Interfaces;

namespace CymaticLabs.InfluxDB.Studio.Controls
{
    /// <summary>
    /// A control that executes an InfluxDB query and displays the results.
    /// </summary>
    public partial class QueryControl : RequestControl
    {
        #region Fields

        // Used for timing operations.
        System.Diagnostics.Stopwatch stopWatch;

        // Used to give resulting rows an ID number
        int resultsCount = 0;

        readonly InfluxQueryIntellisense intellisense = new InfluxQueryIntellisense();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the query editor's text.
        /// </summary>
        public string EditorText
        {
            get { return queryEditor.Text; }
            set { queryEditor.Text = value; }
        }

        #endregion Properties

        #region Constructors

        public QueryControl()
        {
            stopWatch = new System.Diagnostics.Stopwatch();

            InitializeComponent();

            // Clear query results text
            resultsLabel.Text = null;

            queryEditor.ApplyConfiguration(KnownLanguages.SQL);
            queryEditor.BorderStyle = BorderStyle.FixedSingle;
            queryEditor.UseXPStyleBorder = false;
            queryEditor.StatusBarSettings.VisualStyle = Syncfusion.Windows.Forms.Tools.Controls.StatusBar.VisualStyle.Metro;
            queryEditor.ShowHorizontalSplitters = false;
            queryEditor.ShowVerticalSplitters = false;
            queryEditor.FilterAutoCompleteItems = true;
            queryEditor.AutoCompleteSingleLexem = false;
            queryEditor.ContextChoiceOpen += QueryEditor_ContextChoiceOpen;
            queryEditor.ContextChoiceUpdate += QueryEditor_ContextChoiceOpen;
            queryEditor.KeyDown += QueryEditor_KeyDown;
            queryEditor.KeyUp += QueryEditor_KeyUp;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Prompts for a query file and loads it into the editor.
        /// </summary>
        public void OpenQuery()
        {
            if (openQueryDialog.ShowDialog(this) != DialogResult.OK) return;
            EditorText = File.ReadAllText(openQueryDialog.FileName);
        }

        /// <summary>
        /// Prompts for a file and saves the current query text.
        /// </summary>
        public void SaveQuery()
        {
            if (string.IsNullOrWhiteSpace(EditorText))
            {
                MessageBox.Show("Enter a query before saving.", "Save Query",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var connectionName = InfluxDbClient != null ? InfluxDbClient.Connection.Name : "InfluxDB";
            saveQueryDialog.FileName = MakeSafeFileName(connectionName + "_" + Database + "_query.sql");
            if (saveQueryDialog.ShowDialog(this) != DialogResult.OK) return;

            File.WriteAllText(saveQueryDialog.FileName, EditorText);
        }

        /// <summary>
        /// Adds an InfluxQL/SQL line comment to each selected line, or to the
        /// current line when there is no selection.
        /// </summary>
        public void CommentSelectedLines()
        {
            ReplaceSelectedOrCurrentLines(text => Regex.Replace(text, @"(?m)^(?<indent>[ \t]*)", "${indent}-- "));
            queryEditor.Focus();
        }

        /// <summary>
        /// Removes an InfluxQL/SQL line comment from each selected line, or from
        /// the current line when there is no selection.
        /// </summary>
        public void UncommentSelectedLines()
        {
            ReplaceSelectedOrCurrentLines(text => Regex.Replace(text, @"(?m)^(?<indent>[ \t]*)--[ \t]?", "${indent}"));
            queryEditor.Focus();
        }

        /// <summary>
        /// Loads measurement/table names for completion. Field and tag names are loaded lazily
        /// after a FROM clause identifies the active measurement.
        /// </summary>
        public async Task InitializeIntellisenseAsync()
        {
            try
            {
                await intellisense.LoadMeasurementsAsync(InfluxDbClient, Database);
            }
            catch
            {
                // Query execution must remain available if schema discovery is not permitted.
            }
        }

        /// <summary>
        /// Runs the current query against the configured connection and database.
        /// </summary>
        public override async Task ExecuteRequestAsync()
        {
            if (InfluxDbClient == null) throw new Exception("No InfluxDB client available.");

            // Reset the results count
            resultsCount = 0;

            // Match SQL Server Management Studio: run the selected text when a
            // selection exists; otherwise run the complete editor contents.
            var query = GetQueryTextToExecute();
            if (string.IsNullOrWhiteSpace(query))
            {
                MessageBox.Show("Select or enter a query to run.", "Run Query",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool isAggregate = query.IndexOf("group by",
                StringComparison.OrdinalIgnoreCase) >= 0;

            // Clear the current results
            tabControl.Controls.Clear();

            // Start timing...
            stopWatch.Restart();

            // Execute the query
            var results = await InfluxDbClient.QueryAsync(Database, query);

            // Stop timing...
            stopWatch.Stop();

            // If there are results
            if (results != null && results.Count() > 0)
            {
                var tabCount = 0;
                var tabLabel = isAggregate ? "Group" : "Results";

                foreach (var result in results)
                {
                    // Create a new tab page to hold the query results control
                    var tab = new TabPage(string.Format("{0} {1}", tabLabel, ++tabCount));

                    // Create a new query results control
                    var queryResultsControl = new QueryResultsControl();
                    queryResultsControl.InfluxDbClient = InfluxDbClient;
                    queryResultsControl.Database = Database;
                    queryResultsControl.Dock = DockStyle.Fill;
                    tab.Controls.Add(queryResultsControl);

                    // Add the tab to the control
                    tabControl.TabPages.Add(tab);

                    // Render the results and increment the global total
                    resultsCount += queryResultsControl.UpdateResults(result);
                }
            }

            // Show stat results of query
            resultsLabel.Text = string.Format("results: {0}, response time: {1:0} ms", resultsCount, stopWatch.Elapsed.TotalMilliseconds);
        }

        private string GetQueryTextToExecute()
        {
            var hasSelection = !string.IsNullOrEmpty(queryEditor.SelectedText);
            return (hasSelection ? queryEditor.SelectedText : EditorText).Trim();
        }

        private void QueryEditor_ContextChoiceOpen(IContextChoiceController controller)
        {
            controller.Items.Clear();
            controller.UseAutocomplete = true;
            foreach (var item in intellisense.GetItems(InfluxDbClient, EditorText))
                controller.Items.Add(item.Text, item.Description);
        }

        private void QueryEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Space)
            {
                queryEditor.ShowContextChoice();
                e.SuppressKeyPress = true;
            }
        }

        private async void QueryEditor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Space && e.KeyCode != Keys.OemPeriod) return;
            try
            {
                await intellisense.LoadContextSchemaAsync(InfluxDbClient, Database, EditorText);
                if (Regex.IsMatch(queryEditor.CurrentLineText ?? string.Empty,
                    @"\b(SELECT|FROM|WHERE|GROUP\s+BY|ORDER\s+BY)\s+$",
                    RegexOptions.IgnoreCase))
                    queryEditor.ShowContextChoice();
            }
            catch
            {
                // Some accounts cannot read schema metadata; static completion remains available.
            }
        }

        private void ReplaceSelectedOrCurrentLines(Func<string, string> transform)
        {
            if (!string.IsNullOrEmpty(queryEditor.SelectedText))
            {
                queryEditor.SelectedText = transform(queryEditor.SelectedText);
                return;
            }

            var lineNumber = queryEditor.CurrentLine;
            var lineText = queryEditor.CurrentLineText ?? string.Empty;
            queryEditor.SetSelection(1, lineNumber, lineText.Length + 1, lineNumber);
            queryEditor.SelectedText = transform(lineText);
        }

        private static string MakeSafeFileName(string fileName)
        {
            foreach (var invalidCharacter in Path.GetInvalidFileNameChars())
                fileName = fileName.Replace(invalidCharacter, '_');
            return fileName;
        }

        #endregion Methods
    }
}
