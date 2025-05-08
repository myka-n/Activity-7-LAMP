using System;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Activity_7
{
    public static class DatabaseManager
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString;

        public static MySqlConnection GetConnection()
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Database connection string is not configured.");
            }
            return new MySqlConnection(connectionString);
        }

        public static void ExecuteQuery(string query, Action<MySqlCommand> parameterSetter, Action<MySqlDataReader> resultHandler)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    parameterSetter?.Invoke(command);
                    using (var reader = command.ExecuteReader())
                    {
                        resultHandler?.Invoke(reader);
                    }
                }
            }
        }
    }
} 