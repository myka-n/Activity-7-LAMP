using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Activity_7
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Initialize application configuration
            ApplicationConfiguration.Initialize();

            // Define your MySQL connection string
            string connectionString = "server=localhost;user id=root;password=mykz;database=zeereal_artspace;";

            // Attempt to establish a connection to the MySQL database
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MessageBox.Show("Database connection successful!", "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Database connection failed:\n{ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Exit the application if the connection fails
            }

            // Run the main application form
            Application.Run(new Login());
        }
    }
}
