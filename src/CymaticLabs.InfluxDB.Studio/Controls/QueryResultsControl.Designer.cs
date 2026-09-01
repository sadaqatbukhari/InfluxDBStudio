namespace CymaticLabs.InfluxDB.Studio.Controls
{
    partial class QueryResultsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            contextMenuStrip = new Syncfusion.Windows.Forms.Tools.ContextMenuStripEx();
            exportAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exportAllCsvToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            jSONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exportSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exportSelectedCsvToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            jSONToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            resultsGrid = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            splitContainer = new System.Windows.Forms.SplitContainer();
            tagsTextBox = new System.Windows.Forms.TextBox();
            saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)resultsGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { exportAllToolStripMenuItem, exportSelectedToolStripMenuItem });
            contextMenuStrip.MetroColor = System.Drawing.Color.FromArgb(204, 236, 249);
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new System.Drawing.Size(183, 52);
            // 
            // exportAllToolStripMenuItem
            // 
            exportAllToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { exportAllCsvToolStripMenuItem, jSONToolStripMenuItem });
            exportAllToolStripMenuItem.Name = "exportAllToolStripMenuItem";
            exportAllToolStripMenuItem.Size = new System.Drawing.Size(182, 24);
            exportAllToolStripMenuItem.Text = "Export All";
            // 
            // exportAllCsvToolStripMenuItem
            // 
            exportAllCsvToolStripMenuItem.Name = "exportAllCsvToolStripMenuItem";
            exportAllCsvToolStripMenuItem.Size = new System.Drawing.Size(127, 26);
            exportAllCsvToolStripMenuItem.Text = "CSV";
            exportAllCsvToolStripMenuItem.Click += exportAllCsvToolStripMenuItem_Click;
            //
            // jSONToolStripMenuItem
            //
            jSONToolStripMenuItem.Name = "jSONToolStripMenuItem";
            jSONToolStripMenuItem.Size = new System.Drawing.Size(127, 26);
            jSONToolStripMenuItem.Text = "JSON";
            jSONToolStripMenuItem.Click += jSONToolStripMenuItem_Click;
            // 
            // exportSelectedToolStripMenuItem
            // 
            exportSelectedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { exportSelectedCsvToolStripMenuItem, jSONToolStripMenuItem1 });
            exportSelectedToolStripMenuItem.Name = "exportSelectedToolStripMenuItem";
            exportSelectedToolStripMenuItem.Size = new System.Drawing.Size(182, 24);
            exportSelectedToolStripMenuItem.Text = "Export Selected";
            // 
            // exportSelectedCsvToolStripMenuItem
            // 
            exportSelectedCsvToolStripMenuItem.Name = "exportSelectedCsvToolStripMenuItem";
            exportSelectedCsvToolStripMenuItem.Size = new System.Drawing.Size(127, 26);
            exportSelectedCsvToolStripMenuItem.Text = "CSV";
            exportSelectedCsvToolStripMenuItem.Click += exportSelectedCsvToolStripMenuItem_Click;
            //
            // jSONToolStripMenuItem1
            //
            jSONToolStripMenuItem1.Name = "jSONToolStripMenuItem1";
            jSONToolStripMenuItem1.Size = new System.Drawing.Size(127, 26);
            jSONToolStripMenuItem1.Text = "JSON";
            jSONToolStripMenuItem1.Click += jSONToolStripMenuItem1_Click;
            //
            // resultsGrid
            //
            resultsGrid.AccessibleName = "Table";
            resultsGrid.AllowEditing = false;
            resultsGrid.AllowFiltering = true;
            resultsGrid.AllowResizingColumns = true;
            resultsGrid.AllowResizingHiddenColumns = true;
            resultsGrid.ContextMenuStrip = contextMenuStrip;
            resultsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            resultsGrid.FilterPopupMode = Syncfusion.WinForms.GridCommon.FilterPopupMode.AdvancedFilter;
            resultsGrid.Location = new System.Drawing.Point(0, 0);
            resultsGrid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            resultsGrid.Name = "resultsGrid";
            resultsGrid.PreviewRowHeight = 35;
            resultsGrid.Size = new System.Drawing.Size(917, 661);
            resultsGrid.TabIndex = 0;
            // 
            // splitContainer
            // 
            splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer.Location = new System.Drawing.Point(0, 0);
            splitContainer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            splitContainer.Name = "splitContainer";
            splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(tagsTextBox);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(resultsGrid);
            splitContainer.Size = new System.Drawing.Size(917, 705);
            splitContainer.SplitterDistance = 38;
            splitContainer.SplitterWidth = 6;
            splitContainer.TabIndex = 4;
            // 
            // tagsTextBox
            // 
            tagsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            tagsTextBox.Location = new System.Drawing.Point(0, 0);
            tagsTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tagsTextBox.Multiline = true;
            tagsTextBox.Name = "tagsTextBox";
            tagsTextBox.ReadOnly = true;
            tagsTextBox.Size = new System.Drawing.Size(917, 38);
            tagsTextBox.TabIndex = 1;
            // 
            // QueryResultsControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitContainer);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "QueryResultsControl";
            Size = new System.Drawing.Size(917, 705);
            contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)resultsGrid).EndInit();
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel1.PerformLayout();
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion
        private Syncfusion.WinForms.DataGrid.SfDataGrid resultsGrid;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TextBox tagsTextBox;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private Syncfusion.Windows.Forms.Tools.ContextMenuStripEx contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem exportAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportAllCsvToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportSelectedCsvToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jSONToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jSONToolStripMenuItem1;
    }
}
