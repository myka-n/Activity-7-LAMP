using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using OfficeOpenXml;
using System.IO;

namespace Activity_7
{
    public static class ReportService
    {
        public static void GenerateUserReport(string filePath, DateTime startDate, DateTime endDate)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
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
                            u.last_login,
                            u.role,
                            COUNT(a.art_id) as artwork_count
                        FROM users u
                        LEFT JOIN artworks a ON u.user_id = a.user_id
                        WHERE u.is_active = 1
                        AND u.created_at BETWEEN @StartDate AND @EndDate
                        GROUP BY u.user_id
                        ORDER BY u.created_at DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        using (ExcelPackage package = new ExcelPackage())
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Users");
                            
                            // Add headers
                            worksheet.Cells[1, 1].Value = "User ID";
                            worksheet.Cells[1, 2].Value = "Username";
                            worksheet.Cells[1, 3].Value = "Email";
                            worksheet.Cells[1, 4].Value = "Created At";
                            worksheet.Cells[1, 5].Value = "Last Login";
                            worksheet.Cells[1, 6].Value = "Role";
                            worksheet.Cells[1, 7].Value = "Artwork Count";

                            // Add data
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                worksheet.Cells[i + 2, 1].Value = dt.Rows[i]["user_id"];
                                worksheet.Cells[i + 2, 2].Value = dt.Rows[i]["username"];
                                worksheet.Cells[i + 2, 3].Value = dt.Rows[i]["email"];
                                worksheet.Cells[i + 2, 4].Value = Convert.ToDateTime(dt.Rows[i]["created_at"]).ToString("yyyy-MM-dd HH:mm:ss");
                                worksheet.Cells[i + 2, 5].Value = dt.Rows[i]["last_login"] != DBNull.Value ? 
                                    Convert.ToDateTime(dt.Rows[i]["last_login"]).ToString("yyyy-MM-dd HH:mm:ss") : "";
                                worksheet.Cells[i + 2, 6].Value = dt.Rows[i]["role"];
                                worksheet.Cells[i + 2, 7].Value = dt.Rows[i]["artwork_count"];
                            }

                            // Auto-fit columns
                            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                            // Save the file
                            package.SaveAs(new FileInfo(filePath));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error generating user report: " + ex.Message);
                }
            }
        }

        public static void GenerateArtworkReport(string filePath, int? categoryId, DateTime startDate, DateTime endDate)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            a.art_id,
                            a.title,
                            a.description,
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

                        using (ExcelPackage package = new ExcelPackage())
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Artworks");
                            
                            // Add headers
                            worksheet.Cells[1, 1].Value = "Artwork ID";
                            worksheet.Cells[1, 2].Value = "Title";
                            worksheet.Cells[1, 3].Value = "Description";
                            worksheet.Cells[1, 4].Value = "Created At";
                            worksheet.Cells[1, 5].Value = "Artist";
                            worksheet.Cells[1, 6].Value = "Category";

                            // Add data
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                worksheet.Cells[i + 2, 1].Value = dt.Rows[i]["art_id"];
                                worksheet.Cells[i + 2, 2].Value = dt.Rows[i]["title"];
                                worksheet.Cells[i + 2, 3].Value = dt.Rows[i]["description"];
                                worksheet.Cells[i + 2, 4].Value = Convert.ToDateTime(dt.Rows[i]["created_at"]).ToString("yyyy-MM-dd HH:mm:ss");
                                worksheet.Cells[i + 2, 5].Value = dt.Rows[i]["artist_name"];
                                worksheet.Cells[i + 2, 6].Value = dt.Rows[i]["category_name"];
                            }

                            // Auto-fit columns
                            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                            // Save the file
                            package.SaveAs(new FileInfo(filePath));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error generating artwork report: " + ex.Message);
                }
            }
        }
    }
} 