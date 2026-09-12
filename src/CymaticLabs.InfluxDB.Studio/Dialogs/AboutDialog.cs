using System;
using System.Windows.Forms;

namespace CymaticLabs.InfluxDB.Studio.Dialogs
{
    /// <summary>
    /// Application about dialog.
    /// </summary>
    public partial class AboutDialog : Form
    {
        #region Fields

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Constructors

        public AboutDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Event Handlers

        // Form Load
        private void AboutDialog_Load(object sender, EventArgs e)
        {
            // Apply the current version number
            versionLabel.Text = AppForm.Settings.Version;
        }

        // Launch project link
        private void projectLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLink("https://github.com/sadaqatbukhari/InfluxDBStudio");
        }

        // Launch the official InfluxDB 1.x .NET client package link
        private void influxDataNetLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLink("https://www.nuget.org/packages/InfluxDB.Client");
        }

        // Launch the official InfluxDB 3.x .NET client package link
        private void influxDb3ClientLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLink("https://www.nuget.org/packages/InfluxDB3.Client");
        }

        #endregion Event Handlers

        #region Methods

        private static void OpenLink(string url)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url)
            {
                UseShellExecute = true
            });
        }

        #endregion Methods
    }
}
