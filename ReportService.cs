using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using ClosedXML.Excel;
using System.IO;
using System.Windows.Forms;

namespace Activity_7
{
    public class ReportService
    {
        private readonly string _connectionString;

        public ReportService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static void GenerateUserReport(string filePath, DateTime startDate, DateTime endDate)
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString;
                if (string.IsNullOrEmpty(connStr))
                {
                    throw new Exception("Database connection string not found in configuration.");
                }

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    try
                    {
                        conn.Open();
                        string query = @"
                            SELECT 
                                u.user_id,
                                u.username,
                                u.email,
                                u.created_at,
                                u.role,
                                COUNT(a.art_id) as artwork_count
                            FROM users u
                            LEFT JOIN artworks a ON u.user_id = a.user_id
                            WHERE u.created_at BETWEEN @StartDate AND @EndDate
                            GROUP BY u.user_id
                            ORDER BY u.created_at DESC";

                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // Debug: Check if we have data
                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show($"No data found for the period {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}", 
                                    "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                            using (var workbook = new XLWorkbook())
                            {
                                var worksheet = workbook.Worksheets.Add("Users");
                                
                                // Add headers
                                var headerRow = worksheet.Row(1);
                                headerRow.Cell(1).Value = "User ID";
                                headerRow.Cell(2).Value = "Username";
                                headerRow.Cell(3).Value = "Email";
                                headerRow.Cell(4).Value = "Created At";
                                headerRow.Cell(5).Value = "Role";
                                headerRow.Cell(6).Value = "Artwork Count";

                                // Add data
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    try
                                    {
                                        var dataRow = worksheet.Row(i + 2);
                                        dataRow.Cell(1).Value = Convert.ToInt32(dt.Rows[i]["user_id"]);
                                        dataRow.Cell(2).Value = dt.Rows[i]["username"].ToString();
                                        dataRow.Cell(3).Value = dt.Rows[i]["email"].ToString();
                                        dataRow.Cell(4).Value = Convert.ToDateTime(dt.Rows[i]["created_at"]).ToString("yyyy-MM-dd HH:mm:ss");
                                        dataRow.Cell(5).Value = dt.Rows[i]["role"].ToString();
                                        dataRow.Cell(6).Value = Convert.ToInt32(dt.Rows[i]["artwork_count"]);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"Error processing row {i + 1}: {ex.Message}", 
                                            "Data Processing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }

                                // Auto-fit columns
                                worksheet.Columns().AdjustToContents();

                                // Save the file
                                workbook.SaveAs(filePath);
                                MessageBox.Show($"Report generated successfully with {dt.Rows.Count} records.", 
                                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error generating user report: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to generate user report: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void GenerateArtworkReport(string filePath, int? categoryId, DateTime startDate, DateTime endDate)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString;
            if (string.IsNullOrEmpty(connStr))
            {
                throw new Exception("Database connection string not found in configuration.");
            }

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            a.art_id,
                            a.art_title,
                            a.art_description,
                            a.created_at,
                            u.username as artist_name,
                            c.cat_name as category_name
                        FROM artworks a
                        JOIN users u ON a.user_id = u.user_id
                        LEFT JOIN artwork_categories ac ON a.art_id = ac.art_id
                        LEFT JOIN categories c ON ac.cat_id = c.cat_id
                        WHERE a.created_at BETWEEN @StartDate AND @EndDate
                        " + (categoryId.HasValue ? "AND c.cat_id = @CategoryId" : "") + @"
                        ORDER BY a.created_at DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);
                    if (categoryId.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@CategoryId", categoryId.Value);
                    }

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Debug: Check if we have data
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show($"No data found for the period {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}", 
                                "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Artworks");
                            
                            // Add headers
                            var headerRow = worksheet.Row(1);
                            headerRow.Cell(1).Value = "Artwork ID";
                            headerRow.Cell(2).Value = "Title";
                            headerRow.Cell(3).Value = "Description";
                            headerRow.Cell(4).Value = "Created At";
                            headerRow.Cell(5).Value = "Artist";
                            headerRow.Cell(6).Value = "Category";

                            // Add data
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                try
                                {
                                    var dataRow = worksheet.Row(i + 2);
                                    dataRow.Cell(1).Value = Convert.ToInt32(dt.Rows[i]["art_id"]);
                                    dataRow.Cell(2).Value = dt.Rows[i]["art_title"].ToString();
                                    dataRow.Cell(3).Value = dt.Rows[i]["art_description"].ToString();
                                    dataRow.Cell(4).Value = Convert.ToDateTime(dt.Rows[i]["created_at"]).ToString("yyyy-MM-dd HH:mm:ss");
                                    dataRow.Cell(5).Value = dt.Rows[i]["artist_name"].ToString();
                                    dataRow.Cell(6).Value = dt.Rows[i]["category_name"].ToString();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Error processing row {i + 1}: {ex.Message}", 
                                        "Data Processing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }

                            // Auto-fit columns
                            worksheet.Columns().AdjustToContents();

                            // Save the file
                            workbook.SaveAs(filePath);
                            MessageBox.Show($"Report generated successfully with {dt.Rows.Count} records.", 
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error generating artwork report: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
} 