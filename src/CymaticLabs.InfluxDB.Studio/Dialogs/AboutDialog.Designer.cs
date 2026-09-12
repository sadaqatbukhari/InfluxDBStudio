namespace CymaticLabs.InfluxDB.Studio.Dialogs
{
    partial class AboutDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            influxDBLogo = new System.Windows.Forms.PictureBox();
            titleLabel = new System.Windows.Forms.Label();
            versionLabel = new System.Windows.Forms.Label();
            closeButton = new System.Windows.Forms.Button();
            descriptionLabel = new System.Windows.Forms.Label();
            projectLabel = new System.Windows.Forms.Label();
            copyrightLabel = new System.Windows.Forms.Label();
            warranyLabel = new System.Windows.Forms.Label();
            projectLinkLabel = new System.Windows.Forms.LinkLabel();
            influxDataNetLabel = new System.Windows.Forms.Label();
            influxDataNetLinkLabel = new System.Windows.Forms.LinkLabel();
            influxDb3ClientLinkLabel = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)influxDBLogo).BeginInit();
            SuspendLayout();
            // 
            // influxDBLogo
            // 
            influxDBLogo.Image = Properties.Resources.influxdb_logo;
            influxDBLogo.Location = new System.Drawing.Point(16, 18);
            influxDBLogo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            influxDBLogo.Name = "influxDBLogo";
            influxDBLogo.Size = new System.Drawing.Size(220, 254);
            influxDBLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            influxDBLogo.TabIndex = 0;
            influxDBLogo.TabStop = false;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            titleLabel.Location = new System.Drawing.Point(252, 20);
            titleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new System.Drawing.Size(140, 20);
            titleLabel.TabIndex = 1;
            titleLabel.Text = "InfluxDB Studio";
            // 
            // versionLabel
            // 
            versionLabel.AutoSize = true;
            versionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            versionLabel.Location = new System.Drawing.Point(413, 20);
            versionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            versionLabel.Name = "versionLabel";
            versionLabel.Size = new System.Drawing.Size(64, 20);
            versionLabel.TabIndex = 1;
            versionLabel.Text = "0.0.0.0";
            // 
            // closeButton
            // 
            closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            closeButton.Location = new System.Drawing.Point(591, 345);
            closeButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            closeButton.Name = "closeButton";
            closeButton.Size = new System.Drawing.Size(100, 35);
            closeButton.TabIndex = 2;
            closeButton.Text = "Close";
            closeButton.UseVisualStyleBackColor = true;
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new System.Drawing.Point(252, 62);
            descriptionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new System.Drawing.Size(233, 20);
            descriptionLabel.TabIndex = 3;
            descriptionLabel.Text = "Visual InfluxDB Management Tool";
            // 
            // projectLabel
            // 
            projectLabel.AutoSize = true;
            projectLabel.Location = new System.Drawing.Point(252, 138);
            projectLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            projectLabel.Name = "projectLabel";
            projectLabel.Size = new System.Drawing.Size(58, 20);
            projectLabel.TabIndex = 3;
            projectLabel.Text = "Project:";
            // 
            // copyrightLabel
            // 
            copyrightLabel.AutoSize = true;
            copyrightLabel.Location = new System.Drawing.Point(252, 100);
            copyrightLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            copyrightLabel.Name = "copyrightLabel";
            copyrightLabel.Size = new System.Drawing.Size(324, 20);
            copyrightLabel.TabIndex = 3;
            copyrightLabel.Text = "Developed and maintained by Sadaqat Hussain";
            // 
            // warranyLabel
            // 
            warranyLabel.Location = new System.Drawing.Point(252, 223);
            warranyLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            warranyLabel.Name = "warranyLabel";
            warranyLabel.Size = new System.Drawing.Size(439, 94);
            warranyLabel.TabIndex = 3;
            warranyLabel.Text = "The program is provided AS IS with NO WARRANTY OF ANY KIND, INCLUDING THE WARRANTY OF DESIGN, MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.";
            // 
            // projectLinkLabel
            // 
            projectLinkLabel.AutoSize = true;
            projectLinkLabel.Location = new System.Drawing.Point(317, 138);
            projectLinkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            projectLinkLabel.Name = "projectLinkLabel";
            projectLinkLabel.Size = new System.Drawing.Size(297, 20);
            projectLinkLabel.TabIndex = 4;
            projectLinkLabel.TabStop = true;
            projectLinkLabel.Text = "github.com/sadaqatbukhari/InfluxDBStudio";
            projectLinkLabel.LinkClicked += projectLinkLabel_LinkClicked;
            // 
            // influxDataNetLabel
            // 
            influxDataNetLabel.AutoSize = true;
            influxDataNetLabel.Location = new System.Drawing.Point(252, 177);
            influxDataNetLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            influxDataNetLabel.Name = "influxDataNetLabel";
            influxDataNetLabel.Size = new System.Drawing.Size(126, 20);
            influxDataNetLabel.TabIndex = 3;
            influxDataNetLabel.Text = "Official packages:";
            // 
            // influxDataNetLinkLabel
            // 
            influxDataNetLinkLabel.AutoSize = true;
            influxDataNetLinkLabel.Location = new System.Drawing.Point(384, 177);
            influxDataNetLinkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            influxDataNetLinkLabel.Name = "influxDataNetLinkLabel";
            influxDataNetLinkLabel.Size = new System.Drawing.Size(138, 20);
            influxDataNetLinkLabel.TabIndex = 4;
            influxDataNetLinkLabel.TabStop = true;
            influxDataNetLinkLabel.Text = "InfluxDB.Client (1.x)";
            influxDataNetLinkLabel.LinkClicked += influxDataNetLinkLabel_LinkClicked;
            //
            // influxDb3ClientLinkLabel
            //
            influxDb3ClientLinkLabel.AutoSize = true;
            influxDb3ClientLinkLabel.Location = new System.Drawing.Point(535, 177);
            influxDb3ClientLinkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            influxDb3ClientLinkLabel.Name = "influxDb3ClientLinkLabel";
            influxDb3ClientLinkLabel.Size = new System.Drawing.Size(146, 20);
            influxDb3ClientLinkLabel.TabIndex = 4;
            influxDb3ClientLinkLabel.TabStop = true;
            influxDb3ClientLinkLabel.Text = "InfluxDB3.Client (3.x)";
            influxDb3ClientLinkLabel.LinkClicked += influxDb3ClientLinkLabel_LinkClicked;
            // 
            // AboutDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(707, 398);
            Controls.Add(influxDb3ClientLinkLabel);
            Controls.Add(influxDataNetLinkLabel);
            Controls.Add(projectLinkLabel);
            Controls.Add(warranyLabel);
            Controls.Add(copyrightLabel);
            Controls.Add(influxDataNetLabel);
            Controls.Add(projectLabel);
            Controls.Add(descriptionLabel);
            Controls.Add(closeButton);
            Controls.Add(versionLabel);
            Controls.Add(titleLabel);
            Controls.Add(influxDBLogo);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutDialog";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "About InfluxDB Studio";
            Load += AboutDialog_Load;
            ((System.ComponentModel.ISupportInitialize)influxDBLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox influxDBLogo;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label projectLabel;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.Label warranyLabel;
        private System.Windows.Forms.LinkLabel projectLinkLabel;
        private System.Windows.Forms.Label influxDataNetLabel;
        private System.Windows.Forms.LinkLabel influxDataNetLinkLabel;
        private System.Windows.Forms.LinkLabel influxDb3ClientLinkLabel;
    }
}
