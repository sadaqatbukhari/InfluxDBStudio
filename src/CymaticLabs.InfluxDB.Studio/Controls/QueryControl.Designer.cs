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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.queryEditor = new ScintillaNET.Scintilla();
            this.panel1 = new System.Windows.Forms.Panel();
            this.resultsLabel = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.openQueryDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveQueryDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.queryEditor);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl);
            this.splitContainer1.Size = new System.Drawing.Size(916, 645);
            this.splitContainer1.SplitterDistance = 162;
            this.splitContainer1.TabIndex = 0;
            // 
            // queryEditor
            // 
            this.queryEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queryEditor.LexerName = "sql";
            this.queryEditor.Location = new System.Drawing.Point(0, 0);
            this.queryEditor.Name = "queryEditor";
            this.queryEditor.Size = new System.Drawing.Size(916, 128);
            this.queryEditor.TabIndex = 0;
            //
            // panel1
            //
            this.panel1.Controls.Add(this.resultsLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 128);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(916, 34);
            this.panel1.TabIndex = 0;
            //
            // resultsLabel
            // 
            this.resultsLabel.AutoSize = true;
            this.resultsLabel.Location = new System.Drawing.Point(3, 12);
            this.resultsLabel.Name = "resultsLabel";
            this.resultsLabel.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.resultsLabel.Size = new System.Drawing.Size(45, 13);
            this.resultsLabel.TabIndex = 0;
            this.resultsLabel.Text = "results";
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(916, 479);
            this.tabControl.TabIndex = 1;
            //
            // openQueryDialog
            //
            this.openQueryDialog.DefaultExt = "sql";
            this.openQueryDialog.Filter = "Query files|*.sql;*.influxql;*.txt|SQL query files|*.sql|InfluxQL query files|*.influxql|Text files|*.txt|All files|*.*";
            this.openQueryDialog.Title = "Open Query";
            //
            // saveQueryDialog
            //
            this.saveQueryDialog.DefaultExt = "sql";
            this.saveQueryDialog.Filter = "SQL query files|*.sql|InfluxQL query files|*.influxql|Text files|*.txt|All files|*.*";
            this.saveQueryDialog.Title = "Save Query";
            // 
            // QueryControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "QueryControl";
            this.Size = new System.Drawing.Size(916, 645);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private ScintillaNET.Scintilla queryEditor;
        private System.Windows.Forms.Label resultsLabel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.OpenFileDialog openQueryDialog;
        private System.Windows.Forms.SaveFileDialog saveQueryDialog;
    }
}
