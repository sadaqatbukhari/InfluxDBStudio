using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using CymaticLabs.InfluxDB.Data;

namespace CymaticLabs.InfluxDB.Studio.Controls
{
    /// <summary>
    /// Renders the results for a single InfluxDB query.
    /// </summary>
    public partial class QueryResultsControl : UserControl
    {
        #region Fields

        // Used to give resulting rows an ID number
        int resultsCount = 0;

        // A cache of the last results received.
        InfluxDbSeries lastResult;

        // ListView virtual mode asks for visible rows on demand. Keep only a small
        // window of rendered controls while the complete result remains in memory.
        const int VirtualPageSize = 256;
        int cachedFirstIndex = -1;
        ListViewItem[] cachedItems = Array.Empty<ListViewItem>();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="InfluxDB.InfluxDbClient">InfluxDB connection</see> associated
        /// with the control.
        /// </summary>
        public InfluxDbClient InfluxDbClient { get; set; }

        /// <summary>
        /// Gets or sets the name of the database associated with the control.
        /// </summary>
        public string Database { get; set; }

        #endregion Properties

        #region Constructors

        public QueryResultsControl()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Event Handlers

        // Export All -> CSV
        private async void exportAllCsvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await ExportToCsv();
        }

        // Export All -> JSON
        private async void jSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await ExportToJson();
        }

        // Export Selected -> CSV
        private async void exportSelectedCsvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await ExportToCsv(true);
        }

        // Export Selected -> JSON
        private async void jSONToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            await ExportToJson(true);
        }

        private void listView_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {
            CacheVirtualRows(e.StartIndex);
        }

        private void listView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (!IsRowCached(e.ItemIndex))
                CacheVirtualRows(e.ItemIndex);

            e.Item = cachedItems[e.ItemIndex - cachedFirstIndex];
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Clears the current query results from the UI.
        /// </summary>
        public void ClearResults()
        {
            // Clear out current items
            resultsCount = 0;
            lastResult = null;
            tagsTextBox.Text = null;
            listView.BeginUpdate();
            listView.VirtualListSize = 0;
            listView.Columns.Clear();
            ResetVirtualCache();
            listView.EndUpdate();
        }

        /// <summary>
        /// Updates the query control's query results UI with the supplied result.
        /// </summary>
        /// <param name="result">The query result to render.</param>
        /// <returns>The total number of results found.</returns>
        public int UpdateResults(InfluxDbSeries result, bool clear = false)
        {
            if (result == null) throw new ArgumentNullException("result");

            // Clear as needed
            if (clear) ClearResults();

            // Cache the data only. Rows are materialized by ListView virtual mode
            // when they enter the visible scrolling window.
            lastResult = result;
            resultsCount = result.Values.Count;

            // Add tag values to to results
            if (result.Tags.Count > 0)
            {
                splitContainer.Panel1Collapsed = false;
                var tagCount = result.Tags.Count;
                var tagCounter = 0;
                var sb = new StringBuilder();

                foreach (var tag in result.Tags)
                {
                    sb.AppendFormat("{0} = {1}{2}", tag.Key, tag.Value, ++tagCounter < tagCount ? ", " : null);
                }

                tagsTextBox.Text = sb.ToString();
            }
            // Hide tag area if there are no tag values
            else
            {
                splitContainer.Panel1Collapsed = true;
            }

            // Start to update the list view with the new results
            listView.BeginUpdate();
            listView.VirtualListSize = 0;
            listView.Columns.Clear();
            ResetVirtualCache();

            // Build the first column
            var colRecordNum = new ColumnHeader() { Text = "#" };
            listView.Columns.Add(colRecordNum);

            // Build the dynamic columns
            foreach (var c in result.Columns)
            {
                var col = new ColumnHeader();
                col.Text = c;
                listView.Columns.Add(col);
            }

            // Resize each column
            if (listView.Columns.Count > 0)
            {
                var columnWidth = (Width - 12) / listView.Columns.Count;
                if (columnWidth < 96) columnWidth = 96;
                foreach (ColumnHeader col in listView.Columns) col.Width = columnWidth;
            }

            listView.VirtualListSize = resultsCount;
            listView.EndUpdate();

            return resultsCount;
        }

        private void CacheVirtualRows(int requestedIndex)
        {
            if (lastResult == null || lastResult.Values.Count == 0)
            {
                ResetVirtualCache();
                return;
            }

            var pageStart = Math.Max(0, requestedIndex) / VirtualPageSize * VirtualPageSize;
            var firstIndex = Math.Max(0, pageStart - VirtualPageSize);
            var lastIndex = Math.Min(lastResult.Values.Count - 1,
                pageStart + (VirtualPageSize * 2) - 1);

            cachedFirstIndex = firstIndex;
            cachedItems = new ListViewItem[lastIndex - firstIndex + 1];
            for (var index = firstIndex; index <= lastIndex; index++)
                cachedItems[index - firstIndex] = CreateVirtualItem(index);
        }

        private ListViewItem CreateVirtualItem(int rowIndex)
        {
            var row = lastResult.Values[rowIndex];
            var item = new ListViewItem((rowIndex + 1).ToString(CultureInfo.InvariantCulture));

            for (var columnIndex = 0; columnIndex < row.Count; columnIndex++)
            {
                var columnName = columnIndex < lastResult.Columns.Count
                    ? lastResult.Columns[columnIndex]
                    : null;
                item.SubItems.Add(FormatOutputValue(columnName, row[columnIndex]));
            }

            item.Tag = row;
            return item;
        }

        private bool IsRowCached(int rowIndex)
        {
            return cachedFirstIndex >= 0
                && rowIndex >= cachedFirstIndex
                && rowIndex < cachedFirstIndex + cachedItems.Length;
        }

        private void ResetVirtualCache()
        {
            cachedFirstIndex = -1;
            cachedItems = Array.Empty<ListViewItem>();
        }

        private static string FormatOutputValue(string columnName, object value)
        {
            if (value == null) return null;

            var isTimeColumn = string.Equals(columnName, "time", StringComparison.OrdinalIgnoreCase)
                || string.Equals(columnName, "timestamp", StringComparison.OrdinalIgnoreCase);
            if (!isTimeColumn && !(value is DateTime) && !(value is DateTimeOffset))
                return Convert.ToString(value, CultureInfo.CurrentCulture);

            DateTimeOffset timestamp;
            if (value is DateTimeOffset offset)
                timestamp = offset;
            else if (value is DateTime dateTime)
                timestamp = new DateTimeOffset(dateTime);
            else if (!DateTimeOffset.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture),
                CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out timestamp))
                return Convert.ToString(value, CultureInfo.CurrentCulture);

            var timeFormat = AppForm.Settings.TimeFormat;
            if (timeFormat.IndexOf('f') < 0)
                timeFormat = timeFormat.Replace("ss", "ss.fff");

            return timestamp.ToString(AppForm.Settings.DateFormat + " " + timeFormat,
                CultureInfo.CurrentCulture);
        }

        // Exports series data to CSV
        async Task ExportToCsv(bool onlySelected = false)
        {
            try
            {
                // Configure save dialog and open
                saveFileDialog.FileName = string.Format("{0}.csv", InfluxDbClient.Connection.Name + "_" + Database);
                saveFileDialog.Filter = "CSV files|*.csv|All files|*.*";

                if (saveFileDialog.ShowDialog() != DialogResult.OK || lastResult == null) return;

                var result = lastResult;
                var selectedRows = GetSelectedRowIndices();
                var fileName = saveFileDialog.FileName;

                await Task.Run(() =>
                {
                    using (var writer = new StreamWriter(fileName, false, new UTF8Encoding(false)))
                    {
                        writer.WriteLine(string.Join(",", result.Columns.Select(EscapeCsv)));

                        for (var rowIndex = 0; rowIndex < result.Values.Count; rowIndex++)
                        {
                            if (onlySelected && !selectedRows.Contains(rowIndex)) continue;

                            var row = result.Values[rowIndex];
                            var values = new string[result.Columns.Count];
                            for (var columnIndex = 0; columnIndex < values.Length; columnIndex++)
                            {
                                var value = columnIndex < row.Count ? row[columnIndex] : null;
                                values[columnIndex] = EscapeCsv(FormatOutputValue(
                                    result.Columns[columnIndex], value));
                            }
                            writer.WriteLine(string.Join(",", values));
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                AppForm.DisplayException(ex);
            }
        }

        // Streams series data to a JSON array without constructing a second copy
        // of the entire result set in memory.
        async Task ExportToJson(bool onlySelected = false)
        {
            try
            {
                // Configure save dialog and open
                saveFileDialog.FileName = string.Format("{0}.json", InfluxDbClient.Connection.Name + "_" + Database);
                saveFileDialog.Filter = "JSON files|*.json|All files|*.*";

                if (saveFileDialog.ShowDialog() != DialogResult.OK || lastResult == null) return;

                var result = lastResult;
                var selectedRows = GetSelectedRowIndices();
                var fileName = saveFileDialog.FileName;

                await Task.Run(() =>
                {
                    var serializer = JsonSerializer.CreateDefault();
                    using (var streamWriter = new StreamWriter(fileName, false, new UTF8Encoding(false)))
                    using (var jsonWriter = new JsonTextWriter(streamWriter) { Formatting = Formatting.Indented })
                    {
                        jsonWriter.WriteStartArray();
                        for (var rowIndex = 0; rowIndex < result.Values.Count; rowIndex++)
                        {
                            if (onlySelected && !selectedRows.Contains(rowIndex)) continue;

                            var row = result.Values[rowIndex];
                            var item = new Dictionary<string, object>();
                            for (var columnIndex = 0;
                                columnIndex < result.Columns.Count && columnIndex < row.Count;
                                columnIndex++)
                            {
                                item[result.Columns[columnIndex]] = row[columnIndex];
                            }
                            serializer.Serialize(jsonWriter, item);
                        }
                        jsonWriter.WriteEndArray();
                    }
                });
            }
            catch (Exception ex)
            {
                AppForm.DisplayException(ex);
            }
        }

        private HashSet<int> GetSelectedRowIndices()
        {
            var selectedRows = new HashSet<int>();
            foreach (int index in listView.SelectedIndices)
                selectedRows.Add(index);
            return selectedRows;
        }

        private static string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (value.IndexOfAny(new[] { ',', '"', '\r', '\n' }) < 0) return value;
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        #endregion Methods
    }
}
