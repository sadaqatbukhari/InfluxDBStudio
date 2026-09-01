namespace CymaticLabs.InfluxDB.Studio.Controls
{
    partial class QueryControl
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
            Syncfusion.Windows.Forms.Edit.Implementation.Config.Config config1 = new Syncfusion.Windows.Forms.Edit.Implementation.Config.Config();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            queryEditor = new Syncfusion.Windows.Forms.Edit.EditControl();
            panel1 = new System.Windows.Forms.Panel();
            resultsLabel = new System.Windows.Forms.Label();
            tabControl = new System.Windows.Forms.TabControl();
            openQueryDialog = new System.Windows.Forms.OpenFileDialog();
            saveQueryDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)queryEditor).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            //
            // splitContainer1
            //
            splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            //
            // splitContainer1.Panel1
            //
            splitContainer1.Panel1.Controls.Add(queryEditor);
            splitContainer1.Panel1.Controls.Add(panel1);
            splitContainer1.Panel1MinSize = 80;
            //
            // splitContainer1.Panel2
            //
            splitContainer1.Panel2.Controls.Add(tabControl);
            splitContainer1.Panel2MinSize = 100;
            splitContainer1.Size = new System.Drawing.Size(1221, 992);
            splitContainer1.SplitterDistance = 249;
            splitContainer1.SplitterWidth = 11;
            splitContainer1.TabIndex = 0;
            //
            // queryEditor
            //
            queryEditor.AllowZoom = false;
            queryEditor.ChangedLinesMarkingLineColor = System.Drawing.Color.FromArgb(255, 238, 98);
            queryEditor.CodeSnipptSize = new System.Drawing.Size(100, 100);
            queryEditor.Configurator = config1;
            queryEditor.ContextChoiceBackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            queryEditor.ContextChoiceBorderColor = System.Drawing.Color.FromArgb(233, 166, 50);
            queryEditor.ContextChoiceForeColor = System.Drawing.SystemColors.InfoText;
            queryEditor.ContextPromptBackgroundBrush = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(255, 255, 255));
            queryEditor.ContextTooltipBackgroundBrush = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(231, 232, 236));
            queryEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            queryEditor.IndicatorMarginBackColor = System.Drawing.Color.Empty;
            queryEditor.LineNumbersColor = System.Drawing.Color.FromArgb(0, 128, 128);
            queryEditor.Location = new System.Drawing.Point(0, 0);
            queryEditor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            queryEditor.MarkerAreaWidth = 20;
            queryEditor.Name = "queryEditor";
            queryEditor.RenderRightToLeft = false;
            queryEditor.ScrollPosition = new System.Drawing.Point(0, 0);
            queryEditor.SelectionTextColor = System.Drawing.Color.FromArgb(173, 214, 255);
            queryEditor.ShowEndOfLine = false;
            queryEditor.Size = new System.Drawing.Size(1219, 195);
            queryEditor.StatusBarSettings.CoordsPanel.Width = 199;
            queryEditor.StatusBarSettings.EncodingPanel.Width = 133;
            queryEditor.StatusBarSettings.FileNamePanel.Width = 133;
            queryEditor.StatusBarSettings.InsertPanel.Width = 43;
            queryEditor.StatusBarSettings.Offcie2007ColorScheme = Syncfusion.Windows.Forms.Office2007Theme.Blue;
            queryEditor.StatusBarSettings.Offcie2010ColorScheme = Syncfusion.Windows.Forms.Office2010Theme.Blue;
            queryEditor.StatusBarSettings.StatusPanel.Width = 93;
            queryEditor.StatusBarSettings.TextPanel.Width = 285;
            queryEditor.StatusBarSettings.VisualStyle = Syncfusion.Windows.Forms.Tools.Controls.StatusBar.VisualStyle.Default;
            queryEditor.TabIndex = 0;
            queryEditor.Text = "edtCode";
            queryEditor.UseXPStyleBorder = true;
            queryEditor.VisualColumn = 1;
            queryEditor.VScrollMode = Syncfusion.Windows.Forms.Edit.ScrollMode.Immediate;
            queryEditor.ZoomFactor = 1F;
            //
            // panel1
            //
            panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            panel1.Controls.Add(resultsLabel);
            panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            panel1.Location = new System.Drawing.Point(0, 195);
            panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1219, 52);
            panel1.TabIndex = 0;
            //
            // resultsLabel
            //
            resultsLabel.AutoSize = true;
            resultsLabel.Location = new System.Drawing.Point(4, 18);
            resultsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            resultsLabel.Name = "resultsLabel";
            resultsLabel.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            resultsLabel.Size = new System.Drawing.Size(61, 20);
            resultsLabel.TabIndex = 0;
            resultsLabel.Text = "results";
            //
            // tabControl
            //
            tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl.Location = new System.Drawing.Point(0, 0);
            tabControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(1219, 730);
            tabControl.TabIndex = 1;
            //
            // openQueryDialog
            //
            openQueryDialog.DefaultExt = "sql";
            openQueryDialog.Filter = "Query files|*.sql;*.influxql;*.txt|SQL query files|*.sql|InfluxQL query files|*.influxql|Text files|*.txt|All files|*.*";
            openQueryDialog.Title = "Open Query";
            //
            // saveQueryDialog
            //
            saveQueryDialog.DefaultExt = "sql";
            saveQueryDialog.Filter = "SQL query files|*.sql|InfluxQL query files|*.influxql|Text files|*.txt|All files|*.*";
            saveQueryDialog.Title = "Save Query";
            //
            // QueryControl
            //
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "QueryControl";
            Size = new System.Drawing.Size(1221, 992);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)queryEditor).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private Syncfusion.Windows.Forms.Edit.EditControl queryEditor;
        private System.Windows.Forms.Label resultsLabel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.OpenFileDialog openQueryDialog;
        private System.Windows.Forms.SaveFileDialog saveQueryDialog;
    }
}
