using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Activity_7
{
    public static class AuditLogger
    {
        public static void LogAction(int adminId, string action, string details)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        INSERT INTO audit_logs 
                        (admin_id, action, details, timestamp, ip_address) 
                        VALUES 
                        (@AdminId, @Action, @Details, @Timestamp, @IpAddress)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@AdminId", adminId);
                    cmd.Parameters.AddWithValue("@Action", action);
                    cmd.Parameters.AddWithValue("@Details", details);
                    cmd.Parameters.AddWithValue("@Timestamp", DateTime.Now);
                    cmd.Parameters.AddWithValue("@IpAddress", GetClientIP());

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Log to system event log or file if database logging fails
                    System.Diagnostics.EventLog.WriteEntry("Application", 
                        $"Audit Log Error: {ex.Message}", 
                        System.Diagnostics.EventLogEntryType.Error);
                }
            }
        }

        private static string GetClientIP()
        {
            // In a real application, you would get this from the request context
            // For now, we'll return a placeholder
            return "127.0.0.1";
        }
    }
} 