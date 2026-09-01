using Syncfusion.Licensing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CymaticLabs.InfluxDB.Studio
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string licenseKey = Environment.GetEnvironmentVariable("SYNCFUSION_LICENSE_KEY");

            if (!string.IsNullOrEmpty(licenseKey))
            {
                SyncfusionLicenseProvider.RegisterLicense(licenseKey);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AppForm());
        }
    }
}
