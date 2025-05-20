using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Activity_7
{
    public partial class AddUserForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private ComboBox cmbRole;
        private Button btnSave;
        private Button btnCancel;

        public AddUserForm()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeComponent()
        {
            this.Text = "Add New User";
            this.Size = new System.Drawing.Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void InitializeUI()
        {
            // Username
            Label lblUsername = new Label
            {
                Text = "Username:",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(100, 20)
            };

            txtUsername = new TextBox
            {
                Location = new System.Drawing.Point(120, 20),
                Size = new System.Drawing.Size(240, 20)
            };

            // Email
            Label lblEmail = new Label
            {
                Text = "Email:",
                Location = new System.Drawing.Point(20, 50),
                Size = new System.Drawing.Size(100, 20)
            };

            txtEmail = new TextBox
            {
                Location = new System.Drawing.Point(120, 50),
                Size = new System.Drawing.Size(240, 20)
            };

            // Password
            Label lblPassword = new Label
            {
                Text = "Password:",
                Location = new System.Drawing.Point(20, 80),
                Size = new System.Drawing.Size(100, 20)
            };

            txtPassword = new TextBox
            {
                Location = new System.Drawing.Point(120, 80),
                Size = new System.Drawing.Size(240, 20),
                PasswordChar = '•'
            };

            // Confirm Password
            Label lblConfirmPassword = new Label
            {
                Text = "Confirm Password:",
                Location = new System.Drawing.Point(20, 110),
                Size = new System.Drawing.Size(100, 20)
            };

            txtConfirmPassword = new TextBox
            {
                Location = new System.Drawing.Point(120, 110),
                Size = new System.Drawing.Size(240, 20),
                PasswordChar = '•'
            };

            // Role
            Label lblRole = new Label
            {
                Text = "Role:",
                Location = new System.Drawing.Point(20, 140),
                Size = new System.Drawing.Size(100, 20)
            };

            cmbRole = new ComboBox
            {
                Location = new System.Drawing.Point(120, 140),
                Size = new System.Drawing.Size(240, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRole.Items.AddRange(new string[] { "user", "admin" });
            cmbRole.SelectedIndex = 0;

            // Buttons
            btnSave = new Button
            {
                Text = "Save",
                Location = new System.Drawing.Point(120, 180),
                Size = new System.Drawing.Size(100, 30),
                DialogResult = DialogResult.OK
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new System.Drawing.Point(240, 180),
                Size = new System.Drawing.Size(100, 30),
                DialogResult = DialogResult.Cancel
            };

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                lblUsername, txtUsername,
                lblEmail, txtEmail,
                lblPassword, txtPassword,
                lblConfirmPassword, txtConfirmPassword,
                lblRole, cmbRole,
                btnSave, btnCancel
            });
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }

            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // Check if username already exists
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @Username";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Username already exists.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.DialogResult = DialogResult.None;
                            return;
                        }
                    }

                    // Check if email already exists
                    checkQuery = "SELECT COUNT(*) FROM users WHERE email = @Email";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Email already exists.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.DialogResult = DialogResult.None;
                            return;
                        }
                    }

                    // Hash the password
                    string hashedPassword = HashPassword(txtPassword.Text);

                    // Insert new user
                    string insertQuery = @"
                        INSERT INTO users (username, email, password, role, created_at)
                        VALUES (@Username, @Email, @Password, @Role, @CreatedAt)";

                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword);
                        cmd.Parameters.AddWithValue("@Role", cmbRole.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("User added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
} 