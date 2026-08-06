using System;
using System.Windows.Forms;

using CymaticLabs.InfluxDB.Data;

namespace CymaticLabs.InfluxDB.Studio.Dialogs
{
    /// <summary>
    /// Dialog used for creating or updating InfluxDB connection information.
    /// </summary>
    public partial class ConnectionDialog : Form
    {
        #region Fields

        private bool changingServerVersion;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the connection's unique ID.
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// Gets or sets the connection name in the dialog.
        /// </summary>
        public string ConnectionName
        {
            get { return name.Text; }
            set { name.Text = value; }
        }

        /// <summary>
        /// Gets or sets the host address in the dialog.
        /// </summary>
        public string Host
        {
            get { return host.Text; }
            set { host.Text = value; }
        }

        /// <summary>
        /// Gets or sets the host port number.
        /// </summary>
        public int Port
        {
            get { return (int)port.Value; }

            set
            {
                if (value == 0) value = 8086; // set default, don't allow port 0
                port.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the single database to connect to for the connection.
        /// When specifying a single database name database discovery will not occur which
        /// can be helpful if you don't have sufficient privileges to run those commands.
        /// </summary>
        public string Database
        {
            get { return database.Text; }
            set { database.Text = value; }
        }

        /// <summary>
        /// Gets or sets the connection user name.
        /// </summary>
        public string Username
        {
            get { return username.Text; }
            set { username.Text = value; }
        }

        /// <summary>
        /// Gets or sets the connection password.
        /// </summary>
        public string Password
        {
            get { return password.Text; }
            set { password.Text = value; }
        }

        /// <summary>
        /// Gets or sets whether or not to use SSL for the connection.
        /// </summary>
        public bool UseSsl
        {
            get { return useSsl.Checked; }
            set { useSsl.Checked = value; }
        }

        /// <summary>
        /// Gets or sets the selected InfluxDB server generation.
        /// </summary>
        public InfluxDbServerVersion ServerVersion
        {
            get { return (InfluxDbServerVersion)serverVersion.SelectedValue; }
            set
            {
                changingServerVersion = true;
                serverVersion.SelectedValue = value;
                changingServerVersion = false;
                UpdateAuthenticationFields(false);
            }
        }

        /// <summary>
        /// Gets or sets the InfluxDB 3 bearer token.
        /// </summary>
        public string Token
        {
            get { return password.Text; }
            set { password.Text = value; }
        }

        #endregion Properties

        #region Constructors

        public ConnectionDialog()
        {
            InitializeComponent();
            serverVersion.DisplayMember = "Text";
            serverVersion.ValueMember = "Value";
            serverVersion.DataSource = new[]
            {
                new { Text = "InfluxDB 1.x", Value = InfluxDbServerVersion.InfluxDb1 },
                new { Text = "InfluxDB 3.x", Value = InfluxDbServerVersion.InfluxDb3 }
            };
            serverVersion.SelectedValueChanged += delegate
            {
                if (!changingServerVersion) UpdateAuthenticationFields(true);
            };
            ServerVersion = InfluxDbServerVersion.InfluxDb1;
        }

        #endregion Constructors

        #region Event Handlers

        // Handles test connection button click
        private async void testButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a new InfluxDB client
                var client = InfluxDbClientFactory.Create(CreateConnection());

                // Test the connection by requesting a list of databases
                if (!string.IsNullOrWhiteSpace(client.Connection.Database))
                {
                    // If a single database was specified, query the database
                    var response = await client.GetMeasurementNamesAsync(client.Connection.Database);
                    MessageBox.Show("Connection Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // If no single database was specified, query all databases
                    var response = await client.GetDatabaseNamesAsync();
                    MessageBox.Show("Connection Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                AppForm.DisplayException(ex, "Failure");
            }
        }

        // Handles ping connection button click
        private async void pingButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a new InfluxDB client
                var client = InfluxDbClientFactory.Create(CreateConnection());

                // Test the connection by pinging the server
                var response = await client.PingAsync();
                if (!response.Success) throw new Exception("There was an error connecting to the server.");
                var message = string.Format("Response Time: {0:0} ms\nInfluxDB Version: {1}", response.ResponseTime.TotalMilliseconds, response.Version);
                MessageBox.Show(message, "Pong", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppForm.DisplayException(ex, "Failure");
            }
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Resets the dialog's values.
        /// </summary>
        public void ResetValues()
        {
            ConnectionId = null;
            ConnectionName = "New Connection";
            Host = "localhost";
            Port = 8086;
            Database = null;
            Username = null;
            Password = null;
            UseSsl = false;
            ServerVersion = InfluxDbServerVersion.InfluxDb1;
        }

        /// <summary>
        /// Binds the dialog to an InfluxDB connection instance.
        /// </summary>
        /// <param name="connection">The connection to bind to.</param>
        public void BindToConnection(InfluxDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // Pass the connection values to the dialog
            ConnectionId = connection.Id;
            ConnectionName = connection.Name;
            Host = connection.Host;
            Port = connection.Port;
            Database = connection.Database;
            Username = connection.Username;
            Password = connection.Password;
            UseSsl = connection.UseSsl;
            ServerVersion = connection.ServerVersion;
            if (connection.ServerVersion == InfluxDbServerVersion.InfluxDb3) Token = connection.Token;
        }

        /// <summary>
        /// Creates an InfluxDB connection using the dialog's current values.
        /// </summary>
        /// <returns>An InfluxDB connection.</returns>
        public InfluxDbConnection CreateConnection()
        {
            return new InfluxDbConnection(Guid.NewGuid().ToString(), ConnectionName, Host, 
                (ushort)Port, Username, ServerVersion == InfluxDbServerVersion.InfluxDb1 ? Password : null,
                UseSsl, Database, ServerVersion,
                ServerVersion == InfluxDbServerVersion.InfluxDb3 ? Token : null);
        }

        /// <summary>
        /// Updates an InfluxDB connection instance using values from the dialog.
        /// </summary>
        /// <param name="connection">The connection to update.</param>
        public void UpdateConnectionFromDialog(InfluxDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            connection.Id = ConnectionId;
            connection.Name = ConnectionName;
            connection.Host = Host;
            connection.Port = (ushort)Port;
            connection.Database = Database;
            connection.Username = Username;
            connection.Password = Password;
            connection.UseSsl = UseSsl;
            connection.ServerVersion = ServerVersion;
            connection.Token = ServerVersion == InfluxDbServerVersion.InfluxDb3 ? Token : null;
            if (ServerVersion == InfluxDbServerVersion.InfluxDb3)
            {
                connection.Username = null;
                connection.Password = null;
            }
        }

        private void UpdateAuthenticationFields(bool updateDefaultPort)
        {
            if (serverVersion.SelectedValue == null) return;

            var isV3 = ServerVersion == InfluxDbServerVersion.InfluxDb3;
            username.Visible = !isV3;
            usernameLabel.Visible = !isV3;
            passwordLabel.Text = isV3 ? "API Token:" : "Password:";
            databaseInstructions.Text = isV3
                ? "Optionally specify a database. An admin token can discover all databases."
                : "Optionally specify the name of a single database to connect to.\r\nThis can be useful when you don't have admin privileges to list databases.";
            if (updateDefaultPort)
            {
                if (isV3 && Port == 8086) Port = 8181;
                if (!isV3 && Port == 8181) Port = 8086;
            }
        }

        #endregion Methods
    }
}
