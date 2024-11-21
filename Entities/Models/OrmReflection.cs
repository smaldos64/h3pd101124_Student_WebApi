using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public static class OrmReflection
    {
        private static SqlConnection? DatabaseConnection = null;

        public static int InsertObjectToDatabase<T>(this T obj, string tableName)
        {
            int Result = 0;
            // Get the type of the object
            Type type = typeof(T);

            // Get all public instance properties
            PropertyInfo[] properties = type.GetProperties();

            PropertyInfo primaryKeyProperty = properties.FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0);

            // Create a SQL INSERT statement
            StringBuilder sql = new StringBuilder();
            sql.Append($"INSERT INTO {tableName} (");

            // Build the column list and parameter placeholders
            List<string> columnNames = new List<string>();
            List<object> parameters = new List<object>();
            foreach (PropertyInfo property in properties)
            {
                // Filter properties based on your criteria (e.g., exclude navigation properties)
                if (property != primaryKeyProperty && (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string)))
                {
                    columnNames.Add(property.Name);
                    parameters.Add(property.GetValue(obj));
                }
            }

            sql.Append(string.Join(", ", columnNames));
            sql.Append(") VALUES (");
            sql.Append(string.Join(", ", parameters.Select(p => "@p" + parameters.IndexOf(p))));
            sql.Append(");");

            // Execute the SQL query with parameters
            using (SqlConnection connection = new SqlConnection(GetSqlConnectionString()))
            {
                using (SqlCommand command = new SqlCommand(sql.ToString(),
                        connection))
                {
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        command.Parameters.AddWithValue("@p" + i, parameters[i]);
                    }

                    //OpenDatabaseConnection();
                    connection.Open();
                    Result = command.ExecuteNonQuery();
                    if (Result < 0)
                    {
                        Console.WriteLine("Noget gik galt under Save operationen !!!");
                    }
                }
                //CloseDatabaseConnection();
                connection.Close();
            }

            return Result;
        }

        private static void OpenDatabaseConnection()
        {
            if (null == DatabaseConnection)
            {
                DatabaseConnection = new SqlConnection(GetSqlConnectionString());

                DatabaseConnection.Open();
            }
        }

        private static void CloseDatabaseConnection()
        {
            if (null != DatabaseConnection)
            {
                DatabaseConnection.Close();
                DatabaseConnection = null;
            }
        }

        private static string GetSqlConnectionString()
        {
            // Prepare the connection string to Azure SQL Database.
            var sqlConnectionSB = new SqlConnectionStringBuilder
            {
                // Change these values to your values.
                DataSource = "(localdb)\\mssqllocaldb", 
                InitialCatalog = "Student_WebApi_Core_8_0",         
                UserID = "ltpe2",                                   
                Password = "buchwald34",
                TrustServerCertificate = true,
            };
            return sqlConnectionSB.ToString();
        }
    }
}
