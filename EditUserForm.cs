using System;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Activity_7
{
    public partial class EditUserForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private ComboBox cmbRole;
        private Button btnSave;
        private Button btnCancel;
        private Label lblUsername;
        private Label lblEmail;
        private Label lblPassword;
        private Label lblConfirmPassword;
        private Label lblRole;
        private int userId;

        public EditUserForm(int userId)
        {
            this.userId = userId;
            InitializeComponent();
            LoadUserData();
        }

        private void InitializeComponent()
        {
            this.Text = "Edit User";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Create labels
            lblUsername = new Label
            {
                Text = "Username:",
                Location = new Point(20, 20),
                Size = new Size(100, 20)
            };

            lblEmail = new Label
            {
                Text = "Email:",
                Location = new Point(20, 60),
                Size = new Size(100, 20)
            };

            lblPassword = new Label
            {
                Text = "Password:",
                Location = new Point(20, 100),
                Size = new Size(100, 20)
            };

            lblConfirmPassword = new Label
            {
                Text = "Confirm Password:",
                Location = new Point(20, 140),
                Size = new Size(100, 20)
            };

            lblRole = new Label
            {
                Text = "Role:",
                Location = new Point(20, 180),
                Size = new Size(100, 20)
            };

            // Create textboxes
            txtUsername = new TextBox
            {
                Location = new Point(130, 20),
                Size = new Size(200, 20)
            };

            txtEmail = new TextBox
            {
                Location = new Point(130, 60),
                Size = new Size(200, 20)
            };

            txtPassword = new TextBox
            {
                Location = new Point(130, 100),
                Size = new Size(200, 20),
                PasswordChar = '•'
            };

            txtConfirmPassword = new TextBox
            {
                Location = new Point(130, 140),
                Size = new Size(200, 20),
                PasswordChar = '•'
            };

            // Create role combobox
            cmbRole = new ComboBox
            {
                Location = new Point(130, 180),
                Size = new Size(200, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRole.Items.AddRange(new string[] { "user", "admin" });

            // Create buttons
            btnSave = new Button
            {
                Text = "Save",
                Location = new Point(130, 220),
                Size = new Size(80, 30)
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(250, 220),
                Size = new Size(80, 30)
            };
            btnCancel.Click += BtnCancel_Click;

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

        private void LoadUserData()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT username, email, role FROM users WHERE user_id = @UserId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtUsername.Text = reader["username"].ToString();
                            txtEmail.Text = reader["email"].ToString();
                            cmbRole.SelectedItem = reader["role"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading user data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate email format
            try
            {
                var addr = new System.Net.Mail.MailAddress(txtEmail.Text);
            }
            catch
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate password if provided
            if (!string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    // Check for duplicate username
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @Username AND user_id != @UserId";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                    checkCmd.Parameters.AddWithValue("@UserId", userId);
                    int usernameCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (usernameCount > 0)
                    {
                        MessageBox.Show("Username already exists.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Check for duplicate email
                    checkQuery = "SELECT COUNT(*) FROM users WHERE email = @Email AND user_id != @UserId";
                    checkCmd = new MySqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    checkCmd.Parameters.AddWithValue("@UserId", userId);
                    int emailCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (emailCount > 0)
                    {
                        MessageBox.Show("Email already exists.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Update user
                    string updateQuery = "UPDATE users SET username = @Username, email = @Email, role = @Role";
                    if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                    {
                        updateQuery += ", password = @Password";
                    }
                    updateQuery += " WHERE user_id = @UserId";

                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                    updateCmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    updateCmd.Parameters.AddWithValue("@Role", cmbRole.SelectedItem.ToString());
                    updateCmd.Parameters.AddWithValue("@UserId", userId);

                    if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                    {
                        // Hash password
                        using (SHA256 sha256 = SHA256.Create())
                        {
                            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(txtPassword.Text));
                            string hashedPassword = Convert.ToBase64String(hashedBytes);
                            updateCmd.Parameters.AddWithValue("@Password", hashedPassword);
                        }
                    }

                    updateCmd.ExecuteNonQuery();

                    MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
} 