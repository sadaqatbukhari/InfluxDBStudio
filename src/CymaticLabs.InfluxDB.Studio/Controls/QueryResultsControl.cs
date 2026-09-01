using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CymaticLabs.InfluxDB.Data;
using Newtonsoft.Json;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Events;

namespace CymaticLabs.InfluxDB.Studio.Controls
{
    /// <summary>Renders a query result using Syncfusion's virtualized data grid.</summary>
    public partial class QueryResultsControl : UserControl
    {
        private int resultsCount;
        private InfluxDbSeries lastResult;

        public InfluxDbClient InfluxDbClient { get; set; }
        public string Database { get; set; }

        public QueryResultsControl()
        {
            InitializeComponent();
            resultsGrid.AutoGenerateColumns = false;
            resultsGrid.AllowEditing = false;
            resultsGrid.EnableDataVirtualization = true;
            resultsGrid.SelectionMode = GridSelectionMode.Extended;
            resultsGrid.SelectionUnit = SelectionUnit.Row;
            resultsGrid.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            resultsGrid.QueryUnboundColumnInfo += ResultsGrid_QueryUnboundColumnInfo;
        }

        private async void exportAllCsvToolStripMenuItem_Click(object sender, EventArgs e) => await ExportToCsv();
        private async void jSONToolStripMenuItem_Click(object sender, EventArgs e) => await ExportToJson();
        private async void exportSelectedCsvToolStripMenuItem_Click(object sender, EventArgs e) => await ExportToCsv(true);
        private async void jSONToolStripMenuItem1_Click(object sender, EventArgs e) => await ExportToJson(true);

        private void ResultsGrid_QueryUnboundColumnInfo(object sender, QueryUnboundColumnInfoArgs e)
        {
            if (e.UnboundAction != UnboundActions.QueryData || lastResult == null
                || !(e.Record is VirtualResultRow row)) return;

            if (e.Column.MappingName == "__RowNumber")
            {
                e.Value = row.Index + 1;
                return;
            }

            if (!e.Column.MappingName.StartsWith("Column", StringComparison.Ordinal)
                || !int.TryParse(e.Column.MappingName.Substring(6), out var columnIndex)
                || columnIndex >= lastResult.Columns.Count
                || columnIndex >= lastResult.Values[row.Index].Count) return;

            e.Value = FormatOutputValue(lastResult.Columns[columnIndex], lastResult.Values[row.Index][columnIndex]);
        }

        public void ClearResults()
        {
            resultsCount = 0;
            lastResult = null;
            tagsTextBox.Text = null;
            resultsGrid.DataSource = null;
            resultsGrid.Columns.Clear();
        }

        public int UpdateResults(InfluxDbSeries result, bool clear = false)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            if (clear) ClearResults();

            lastResult = result;
            resultsCount = result.Values.Count;
            if (result.Tags.Count > 0)
            {
                splitContainer.Panel1Collapsed = false;
                tagsTextBox.Text = string.Join(", ", result.Tags.Select(tag => $"{tag.Key} = {tag.Value}"));
            }
            else
            {
                splitContainer.Panel1Collapsed = true;
            }

            resultsGrid.DataSource = null;
            resultsGrid.Columns.Clear();
            resultsGrid.Columns.Add(new GridUnboundColumn
            {
                MappingName = "__RowNumber",
                HeaderText = "#",
                MinimumWidth = 64
            });

            for (var columnIndex = 0; columnIndex < result.Columns.Count; columnIndex++)
            {
                resultsGrid.Columns.Add(new GridUnboundColumn
                {
                    MappingName = "Column" + columnIndex.ToString(CultureInfo.InvariantCulture),
                    HeaderText = result.Columns[columnIndex],
                    MinimumWidth = 96
                });
            }

            // This list produces lightweight row handles on demand. The complete raw result
            // remains in its original in-memory representation without creating grid rows.
            resultsGrid.DataSource = new VirtualResultRows(resultsCount);
            return resultsCount;
        }

        private static string FormatOutputValue(string columnName, object value)
        {
            if (value == null) return null;
            var isTimeColumn = string.Equals(columnName, "time", StringComparison.OrdinalIgnoreCase)
                || string.Equals(columnName, "timestamp", StringComparison.OrdinalIgnoreCase);
            if (!isTimeColumn && !(value is DateTime) && !(value is DateTimeOffset))
                return Convert.ToString(value, CultureInfo.CurrentCulture);

            DateTimeOffset timestamp;
            if (value is DateTimeOffset offset) timestamp = offset;
            else if (value is DateTime dateTime) timestamp = new DateTimeOffset(dateTime);
            else if (!DateTimeOffset.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture),
                CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out timestamp))
                return Convert.ToString(value, CultureInfo.CurrentCulture);

            var timeFormat = AppForm.Settings.TimeFormat;
            if (timeFormat.IndexOf('f') < 0) timeFormat = timeFormat.Replace("ss", "ss.fff");
            return timestamp.ToString(AppForm.Settings.DateFormat + " " + timeFormat, CultureInfo.CurrentCulture);
        }

        private async Task ExportToCsv(bool onlySelected = false)
        {
            try
            {
                saveFileDialog.FileName = $"{InfluxDbClient.Connection.Name}_{Database}.csv";
                saveFileDialog.Filter = "CSV files|*.csv|All files|*.*";
                if (saveFileDialog.ShowDialog() != DialogResult.OK || lastResult == null) return;

                var result = lastResult;
                var selectedRows = GetSelectedRowIndices();
                var fileName = saveFileDialog.FileName;
                await Task.Run(() =>
                {
                    using var writer = new StreamWriter(fileName, false, new UTF8Encoding(false));
                    writer.WriteLine(string.Join(",", result.Columns.Select(EscapeCsv)));
                    for (var rowIndex = 0; rowIndex < result.Values.Count; rowIndex++)
                    {
                        if (onlySelected && !selectedRows.Contains(rowIndex)) continue;
                        var row = result.Values[rowIndex];
                        var values = new string[result.Columns.Count];
                        for (var columnIndex = 0; columnIndex < values.Length; columnIndex++)
                        {
                            var value = columnIndex < row.Count ? row[columnIndex] : null;
                            values[columnIndex] = EscapeCsv(FormatOutputValue(result.Columns[columnIndex], value));
                        }
                        writer.WriteLine(string.Join(",", values));
                    }
                });
            }
            catch (Exception ex) { AppForm.DisplayException(ex); }
        }

        private async Task ExportToJson(bool onlySelected = false)
        {
            try
            {
                saveFileDialog.FileName = $"{InfluxDbClient.Connection.Name}_{Database}.json";
                saveFileDialog.Filter = "JSON files|*.json|All files|*.*";
                if (saveFileDialog.ShowDialog() != DialogResult.OK || lastResult == null) return;

                var result = lastResult;
                var selectedRows = GetSelectedRowIndices();
                var fileName = saveFileDialog.FileName;
                await Task.Run(() =>
                {
                    var serializer = JsonSerializer.CreateDefault();
                    using var streamWriter = new StreamWriter(fileName, false, new UTF8Encoding(false));
                    using var jsonWriter = new JsonTextWriter(streamWriter) { Formatting = Formatting.Indented };
                    jsonWriter.WriteStartArray();
                    for (var rowIndex = 0; rowIndex < result.Values.Count; rowIndex++)
                    {
                        if (onlySelected && !selectedRows.Contains(rowIndex)) continue;
                        var row = result.Values[rowIndex];
                        var item = new Dictionary<string, object>();
                        for (var columnIndex = 0; columnIndex < result.Columns.Count && columnIndex < row.Count; columnIndex++)
                            item[result.Columns[columnIndex]] = row[columnIndex];
                        serializer.Serialize(jsonWriter, item);
                    }
                    jsonWriter.WriteEndArray();
                });
            }
            catch (Exception ex) { AppForm.DisplayException(ex); }
        }

        private HashSet<int> GetSelectedRowIndices() => resultsGrid.SelectedItems
            .OfType<VirtualResultRow>().Select(row => row.Index).ToHashSet();

        private static string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (value.IndexOfAny(new[] { ',', '"', '\r', '\n' }) < 0) return value;
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        private sealed class VirtualResultRow
        {
            public VirtualResultRow(int index) { Index = index; }
            public int Index { get; }
        }

        private sealed class VirtualResultRows : IList<VirtualResultRow>
        {
            private readonly int count;
            public VirtualResultRows(int count) { this.count = count; }
            public int Count => count;
            public bool IsReadOnly => true;
            public VirtualResultRow this[int index]
            {
                get
                {
                    if (index < 0 || index >= count) throw new ArgumentOutOfRangeException(nameof(index));
                    return new VirtualResultRow(index);
                }
                set => throw new NotSupportedException();
            }
            public IEnumerator<VirtualResultRow> GetEnumerator()
            {
                for (var index = 0; index < count; index++) yield return new VirtualResultRow(index);
            }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
            public bool Contains(VirtualResultRow value) => value != null && value.Index >= 0 && value.Index < count;
            public int IndexOf(VirtualResultRow value) => value?.Index ?? -1;
            public void CopyTo(VirtualResultRow[] array, int index)
            {
                for (var rowIndex = 0; rowIndex < count; rowIndex++) array[index + rowIndex] = new VirtualResultRow(rowIndex);
            }
            public void Add(VirtualResultRow value) => throw new NotSupportedException();
            public void Clear() => throw new NotSupportedException();
            public void Insert(int index, VirtualResultRow value) => throw new NotSupportedException();
            public bool Remove(VirtualResultRow value) => throw new NotSupportedException();
            public void RemoveAt(int index) => throw new NotSupportedException();
        }
    }
}
