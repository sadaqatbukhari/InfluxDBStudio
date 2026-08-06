using System;

namespace CymaticLabs.InfluxDB.Data
{
    /// <summary>
    /// Factory class for creating <see cref="InfluxDbClient">InfluxDB clients</see>.
    /// </summary>
    public static class InfluxDbClientFactory
    {
        #region Fields

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Constructors

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates a new <see cref="InfluxDbClient">InfluxDB client</see> using the default client implementation.
        /// </summary>
        /// <param name="connection">The connection information to use when creating the client.</param>
        /// <returns>An InfluxDB client that can be used to communicate with the InfluxDB server API.</returns>
        public static InfluxDbClient Create(InfluxDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            return connection.ServerVersion == InfluxDbServerVersion.InfluxDb3
                ? (InfluxDbClient)new InfluxDb3Client(connection)
                : new OfficialInfluxDb1Client(connection);
        }

        #endregion Methods
    }
}
