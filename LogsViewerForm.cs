using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Activity_7
{
    public partial class LogsViewerForm : Form
    {
        private TabControl tabControl;
        private DataGridView dgvAuditLogs;
        private DataGridView dgvUserActivityLogs;
        private DataGridView dgvArtworkLogs;
        private Button btnRefresh;

        public LogsViewerForm()
        {
            InitializeComponent();
            InitializeUI();
            LoadAllLogs();
        }

        private void InitializeComponent()
        {
            this.Text = "System Logs Viewer";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeUI()
        {
            // Create Tab Control
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill
            };

            // Create tabs
            var tabAudit = new TabPage("Audit Logs")
            {
                Margin = new Padding(10)
            };
            var tabUserActivity = new TabPage("User Activity")
            {
                Margin = new Padding(10)
            };
            var tabArtwork = new TabPage("Artwork Logs")
            {
                Margin = new Padding(10)
            };

            // Create DataGridViews
            dgvAuditLogs = CreateDataGridView();
            dgvUserActivityLogs = CreateDataGridView();
            dgvArtworkLogs = CreateDataGridView();

            // Add DataGridViews to tabs
            tabAudit.Controls.Add(dgvAuditLogs);
            tabUserActivity.Controls.Add(dgvUserActivityLogs);
            tabArtwork.Controls.Add(dgvArtworkLogs);

            // Add tabs to TabControl
            tabControl.TabPages.AddRange(new TabPage[] { tabAudit, tabUserActivity, tabArtwork });

            // Create Refresh button
            btnRefresh = new Button
            {
                Text = "Refresh",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            btnRefresh.Click += BtnRefresh_Click;

            // Add controls to form
            this.Controls.AddRange(new Control[] { tabControl, btnRefresh });
        }

        private DataGridView CreateDataGridView()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true
            };
        }

        private void LoadAllLogs()
        {
            LoadAuditLogs();
            LoadUserActivityLogs();
            LoadArtworkLogs();
        }

        private void LoadAuditLogs()
        {
            string query = @"
                SELECT 
                    al.log_id,
                    u.username as admin_username,
                    al.action,
                    al.details,
                    al.timestamp,
                    al.ip_address
                FROM audit_logs al
                JOIN users u ON al.admin_id = u.user_id
                ORDER BY al.timestamp DESC";

            LoadDataIntoGrid(dgvAuditLogs, query);
        }

        private void LoadUserActivityLogs()
        {
            string query = @"
                SELECT 
                    ual.log_id,
                    u.username,
                    ual.activity_type,
                    ual.timestamp
                FROM user_activity_logs ual
                JOIN users u ON ual.user_id = u.user_id
                ORDER BY ual.timestamp DESC";

            LoadDataIntoGrid(dgvUserActivityLogs, query);
        }

        private void LoadArtworkLogs()
        {
            string query = @"
                SELECT 
                    al.log_id,
                    a.art_title,
                    u.username as artist,
                    al.action,
                    al.timestamp
                FROM artwork_logs al
                JOIN artworks a ON al.art_id = a.art_id
                JOIN users u ON a.user_id = u.user_id
                ORDER BY al.timestamp DESC";

            LoadDataIntoGrid(dgvArtworkLogs, query);
        }

        private void LoadDataIntoGrid(DataGridView grid, string query)
        {
            try
            {
                using (var conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString))
                {
                    conn.Open();
                    using (var adapter = new MySqlDataAdapter(query, conn))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        grid.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading logs: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadAllLogs();
        }
    }
} 