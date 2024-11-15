using System.Data.SqlClient;

namespace Student_WebApi_ADO_NET.Tools
{
    public class DatabaseInterface
    {
        public static string GetSqlConnectionString()
        {
            // Prepare the connection string to Azure SQL Database.
            var sqlConnectionSB = new SqlConnectionStringBuilder
            {
                // Change these values to your values.
                DataSource = "(localdb)\\mssqllocaldb", //["Server"]
                InitialCatalog = "Student_WebApi_Core_8_0",                                       //["Database"]
                UserID = "ltpe2",                                          // "@yourservername"  as suffix sometimes.
                Password = "buchwald34",
                // Adjust these values if you like. (ADO.NET 4.5.1 or later.)
                ConnectRetryCount = 3,
                ConnectRetryInterval = 10, // Seconds.
                                           // Leave these values as they are.
                IntegratedSecurity = false,
                //Encrypt = true,
                TrustServerCertificate = true,
                ConnectTimeout = 30
            };
            return sqlConnectionSB.ToString();
        }

        public static List<T> ExecuteDatabaseReadCommand<T>(string ReadCommand)
        {
            List<T> GenericList = new List<T>();

            using var sqlConnection = new SqlConnection(GetSqlConnectionString());
            using var dbCommand = sqlConnection.CreateCommand();

            dbCommand.CommandText = ReadCommand;

            //dbCommand.ExecuteNonQuery();

            sqlConnection.Open();
            var dataReader = dbCommand.ExecuteReader();

            while (dataReader.Read())
            {
                //Console.WriteLine(
                //    "{0}\t{1}",
                //    dataReader.GetString(0),
                //    dataReader.GetString(1)
                //);
                var Temp = Convert.ToInt32(dataReader.GetString(0));
                //GenericList.Add();
            }
            return null;
        }
    }
}
