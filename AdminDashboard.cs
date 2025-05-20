using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;

namespace Activity_7
{
    public partial class AdminDashboard : Form
    {
        private DataGridView dgvUsers;
        private Button btnRefresh;
        private Button btnAddUser;
        private Button btnEditUser;
        private Button btnDeleteUser;
        private Button btnGenerateUserReport;
        private Button btnGenerateArtworkReport;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private ComboBox cmbCategories;
        private Button btnViewLogs;
        private Button btnLogout;
        private int currentAdminId;

        public AdminDashboard(int adminId)
        {
            this.currentAdminId = adminId;
            InitializeComponent();
            InitializeUI();
            LoadUsers();
            LoadCategories();
        }

        private void InitializeComponent()
        {
            this.Text = "Admin Dashboard";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeUI()
        {
            // Create DataGridView
            dgvUsers = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(940, 300),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true
            };

            // Create buttons
            btnRefresh = new Button
            {
                Text = "Refresh",
                Location = new Point(20, 340),
                Size = new Size(100, 30)
            };
            btnRefresh.Click += BtnRefresh_Click;

            btnAddUser = new Button
            {
                Text = "Add User",
                Location = new Point(140, 340),
                Size = new Size(100, 30)
            };
            btnAddUser.Click += BtnAddUser_Click;

            btnEditUser = new Button
            {
                Text = "Edit User",
                Location = new Point(260, 340),
                Size = new Size(100, 30)
            };
            btnEditUser.Click += BtnEditUser_Click;

            btnDeleteUser = new Button
            {
                Text = "Delete User",
                Location = new Point(380, 340),
                Size = new Size(100, 30)
            };
            btnDeleteUser.Click += BtnDeleteUser_Click;

            // Create date pickers
            dtpStartDate = new DateTimePicker
            {
                Location = new Point(20, 400),
                Size = new Size(200, 30),
                Format = DateTimePickerFormat.Short
            };

            dtpEndDate = new DateTimePicker
            {
                Location = new Point(240, 400),
                Size = new Size(200, 30),
                Format = DateTimePickerFormat.Short
            };

            // Create category combo box
            cmbCategories = new ComboBox
            {
                Location = new Point(460, 400),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Create report buttons
            btnGenerateUserReport = new Button
            {
                Text = "Generate User Report",
                Location = new Point(20, 450),
                Size = new Size(200, 30)
            };
            btnGenerateUserReport.Click += BtnGenerateUserReport_Click;

            btnGenerateArtworkReport = new Button
            {
                Text = "Generate Artwork Report",
                Location = new Point(240, 450),
                Size = new Size(200, 30)
            };
            btnGenerateArtworkReport.Click += BtnGenerateArtworkReport_Click;

            // Add View Logs button
            btnViewLogs = new Button
            {
                Text = "View Logs",
                Location = new Point(380, 450),
                Size = new Size(200, 30)
            };
            btnViewLogs.Click += BtnViewLogs_Click;

            // Add Logout button
            btnLogout = new Button
            {
                Text = "Logout",
                Location = new Point(600, 450),
                Size = new Size(200, 30),
                BackColor = Color.LightCoral,
                ForeColor = Color.White
            };
            btnLogout.Click += BtnLogout_Click;

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                dgvUsers,
                btnRefresh,
                btnAddUser,
                btnEditUser,
                btnDeleteUser,
                dtpStartDate,
                dtpEndDate,
                cmbCategories,
                btnGenerateUserReport,
                btnGenerateArtworkReport,
                btnViewLogs,
                btnLogout
            });
        }

        private void LoadUsers()
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
                            u.role,
                            COUNT(a.art_id) as artwork_count
                        FROM users u
                        LEFT JOIN artworks a ON u.user_id = a.user_id
                        GROUP BY u.user_id
                        ORDER BY u.user_id DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvUsers.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading users: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadCategories()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT cat_id, cat_name FROM categories ORDER BY cat_name";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbCategories.DisplayMember = "cat_name";
                    cmbCategories.ValueMember = "cat_id";
                    cmbCategories.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading categories: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            int adminId = GetCurrentAdminId();
            
            using (var addUserForm = new AddUserForm())
            {
                if (addUserForm.ShowDialog() == DialogResult.OK)
                {
                    LoadUsers(); // Refresh the user list
                    AuditLogger.LogAction(adminId, "ADD_USER", "Added new user");
                }
            }
        }

        private void BtnEditUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to edit.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int adminId = GetCurrentAdminId();
            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["user_id"].Value);
            string username = dgvUsers.SelectedRows[0].Cells["username"].Value.ToString();

            using (var editUserForm = new EditUserForm(userId))
            {
                if (editUserForm.ShowDialog() == DialogResult.OK)
                {
                    LoadUsers(); // Refresh the user list
                    AuditLogger.LogAction(adminId, "EDIT_USER", $"Edited user {username} (ID: {userId})");
                }
            }
        }

        private void BtnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to delete.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int adminId = GetCurrentAdminId();
            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["user_id"].Value);
            string username = dgvUsers.SelectedRows[0].Cells["username"].Value.ToString();

            var result = MessageBox.Show(
                $"Are you sure you want to delete user '{username}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM users WHERE user_id = @UserId";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("User deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadUsers();
                        AuditLogger.LogAction(adminId, "DELETE_USER", $"Deleted user {username} (ID: {userId})");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private int GetCurrentAdminId()
        {
            return currentAdminId;
        }

        private void BtnGenerateUserReport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files|*.xlsx";
                saveDialog.Title = "Save User Report";
                saveDialog.FileName = $"UserReport_{DateTime.Now:yyyyMMdd}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ReportService.GenerateUserReport(saveDialog.FileName, dtpStartDate.Value, dtpEndDate.Value);
                        MessageBox.Show("User report generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error generating user report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnGenerateArtworkReport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files|*.xlsx";
                saveDialog.Title = "Save Artwork Report";
                saveDialog.FileName = $"ArtworkReport_{DateTime.Now:yyyyMMdd}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        int? categoryId = cmbCategories.SelectedValue != null ? Convert.ToInt32(cmbCategories.SelectedValue) : (int?)null;
                        ReportService.GenerateArtworkReport(saveDialog.FileName, categoryId, dtpStartDate.Value, dtpEndDate.Value);
                        MessageBox.Show("Artwork report generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error generating artwork report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnViewLogs_Click(object sender, EventArgs e)
        {
            using (var logsForm = new LogsViewerForm())
            {
                logsForm.ShowDialog();
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                // Log the logout activity
                string username = GetCurrentAdminUsername();
                
                // Log to user activity logs
                string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"
                        INSERT INTO user_activity_logs 
                        (user_id, activity_type, timestamp) 
                        VALUES 
                        (@UserId, 'LOGOUT', @Timestamp)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", currentAdminId);
                    cmd.Parameters.AddWithValue("@Timestamp", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }

                // Show confirmation message
                MessageBox.Show("Logged out successfully.", "Logout", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Open login form and close dashboard
                var loginForm = new Login();
                loginForm.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during logout: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetCurrentAdminUsername()
        {
            string username = "Admin"; // Default value
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT username FROM users WHERE user_id = @UserId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", GetCurrentAdminId());
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        username = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error getting admin username: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return username;
        }
    }
} 