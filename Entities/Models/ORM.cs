using Microsoft.Data.SqlClient;
using System;
using System.Text;

namespace Entities.Models
{
    public abstract class ORMField
    {
        protected Func<ORM, string> Getter { get; set; }
        public ORMField(Func<ORM, string> getter)
        {
            Getter = getter;
        }
        public abstract string GetSQLValue(ORM orm);
    }
    public class ORMInt : ORMField
    {
        public ORMInt(Func<ORM, string> getter) : base(getter)
        {

        }
        public override string GetSQLValue(ORM orm)
        {
            return Getter(orm);
        }
    }
    public abstract class ORM
    {
        //table             //property
        static Dictionary<string, Dictionary<string, ORMField>> tables = new Dictionary<string, Dictionary<string, ORMField>>();
        static Dictionary<string, ORMField> primary_keys = new Dictionary<string, ORMField>();

        protected static void Int(string tableName, string propertyName, Func<ORM, string> getter)
        {
            if (!tables.ContainsKey(tableName))
            {
                tables[tableName] = new Dictionary<string, ORMField>();
            }
            tables[tableName][propertyName] = new ORMInt(getter);
        }

        protected static void PrimaryKey(string tableName, string propertyName)
        {
            primary_keys[tableName] = tables[tableName][propertyName];
        }

        public abstract string TableName();
        public int Insert()
        {
            string TABLE_NAME = TableName();
            string fields = "";
            string values = "";
            //Get primary key from this table
            ORMField primaryKey = primary_keys[TABLE_NAME];

            foreach (KeyValuePair<string, ORMField> kv
                in tables[TABLE_NAME])
            {

                string propertyName = kv.Key;
                ORMField valueType = kv.Value;
                if (valueType.Equals(primaryKey)) continue;

                fields += "," + propertyName;
                values += ",'" + valueType.GetSQLValue(this) + "'";
            }

            fields = fields.Substring(1);
            values = values.Substring(1);
            string sql = $@"INSERT INTO {TABLE_NAME} 
                ({fields}) 
                VALUES 
                ({values})";
            Console.WriteLine(sql);

            var sqlConnection = new SqlConnection(GetSqlConnectionString());
            
            sqlConnection.Open();

            SqlCommand dbCommand = new SqlCommand(sql, sqlConnection);

            int Result = dbCommand.ExecuteNonQuery();
            if (Result < 0)
            {
                Console.WriteLine("Noget gik galt under Save operationen !!!");
            }
            sqlConnection.Close();

            return (Result);
        }
        public int Update()
        {
            string TABLE_NAME = TableName();
            ORMField primaryKey = primary_keys[TABLE_NAME];
            string pk_name = "";

            StringBuilder stringBuilder = new StringBuilder();

            foreach (KeyValuePair<string, ORMField> kv in tables[TABLE_NAME])
            {
                string propertyName = kv.Key;
                ORMField valueType = kv.Value;
                if (valueType.Equals(primaryKey))
                {
                    pk_name = propertyName;
                    continue;
                }

                stringBuilder.Append("," + propertyName + "=" + "'" + valueType.GetSQLValue(this) + "'");
            }

            String assignments = stringBuilder.ToString();
            assignments = assignments.Substring(1);
            int pk_value = int.Parse(primaryKey.GetSQLValue(this));

            string sql = $@"UPDATE {TABLE_NAME} 
                SET {assignments}
                WHERE {pk_name}={pk_value}";

            Console.WriteLine(sql);

            var sqlConnection = new SqlConnection(GetSqlConnectionString());

            sqlConnection.Open();

            SqlCommand dbCommand = new SqlCommand(sql, sqlConnection);

            int Result = dbCommand.ExecuteNonQuery();
            if (Result < 0)
            {
                Console.WriteLine("Noget gik galt under Update operationen !!!");
            }
            sqlConnection.Close();

            return (Result);
        }

        public void DeleteDictionariesAfterDatabaseTransaction()
        {
            tables.Clear();
            primary_keys.Clear();
        }

        private static string GetSqlConnectionString()
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
                //ConnectRetryCount = 3,  // LTPE
                //ConnectRetryInterval = 10, // Seconds. LTPE
                                           // Leave these values as they are.
                //IntegratedSecurity = false, // LTPE
                //Encrypt = true,
                TrustServerCertificate = true,
                //ConnectTimeout = 30        // LTPE
            };
            return sqlConnectionSB.ToString();
        }
    }
}