using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Drawing;
using System.Configuration;

namespace Activity_7
{
    public partial class ResetPassword : Form
    {
        private string resetToken;
        private int userId;
        private TextBox newPasswordBox;
        private TextBox confirmPasswordBox;
        private Button resetButton;
        private Label statusLabel;

        public ResetPassword(string token)
        {
            this.resetToken = token;
            InitializeComponents();
            ValidateToken(); // Validate token immediately
        }

        private void InitializeComponents()
        {
            this.Text = "Reset Password";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label titleLabel = new Label
            {
                Text = "Reset Your Password",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(360, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label newPasswordLabel = new Label
            {
                Text = "New Password:",
                Location = new Point(20, 70),
                Size = new Size(100, 20)
            };

            newPasswordBox = new TextBox
            {
                Location = new Point(20, 100),
                Size = new Size(340, 25),
                PasswordChar = '•',
                Font = new Font("Arial", 10)
            };

            Label confirmPasswordLabel = new Label
            {
                Text = "Confirm Password:",
                Location = new Point(20, 140),
                Size = new Size(100, 20)
            };

            confirmPasswordBox = new TextBox
            {
                Location = new Point(20, 170),
                Size = new Size(340, 25),
                PasswordChar = '•',
                Font = new Font("Arial", 10)
            };

            resetButton = new Button
            {
                Text = "Reset Password",
                Location = new Point(20, 220),
                Size = new Size(340, 35),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            statusLabel = new Label
            {
                Location = new Point(20, 270),
                Size = new Size(340, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Red
            };

            resetButton.Click += ResetButton_Click;

            this.Controls.AddRange(new Control[] { 
                titleLabel, 
                newPasswordLabel, 
                newPasswordBox, 
                confirmPasswordLabel, 
                confirmPasswordBox, 
                resetButton,
                statusLabel 
            });
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(newPasswordBox.Text) || string.IsNullOrEmpty(confirmPasswordBox.Text))
            {
                statusLabel.Text = "Please fill in all fields.";
                return;
            }

            if (newPasswordBox.Text != confirmPasswordBox.Text)
            {
                statusLabel.Text = "Passwords do not match.";
                return;
            }

            if (newPasswordBox.Text.Length < 8)
            {
                statusLabel.Text = "Password must be at least 8 characters long.";
                return;
            }

            if (UpdatePassword(newPasswordBox.Text))
            {
                MessageBox.Show("Password has been reset successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                statusLabel.Text = "Failed to reset password. Please try again.";
            }
        }

        private bool ValidateToken()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT user_id FROM password_reset_tokens 
                                   WHERE token = @Token AND expiry_date > NOW() 
                                   AND used = 0";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Token", resetToken);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        userId = Convert.ToInt32(result);
                        return true;
                    }
                    else
                    {
                        statusLabel.Text = "Invalid or expired reset token.";
                        resetButton.Enabled = false;
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    statusLabel.Text = "Error validating token. Please try again.";
                    resetButton.Enabled = false;
                    return false;
                }
            }
        }

        private bool UpdatePassword(string newPassword)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    using (MySqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Update password
                            string updatePasswordQuery = "UPDATE users SET password = @Password WHERE user_id = @UserId";
                            MySqlCommand updateCmd = new MySqlCommand(updatePasswordQuery, conn, transaction);
                            updateCmd.Parameters.AddWithValue("@Password", HashPassword(newPassword));
                            updateCmd.Parameters.AddWithValue("@UserId", userId);
                            updateCmd.ExecuteNonQuery();

                            // Mark token as used
                            string updateTokenQuery = "UPDATE password_reset_tokens SET used = 1 WHERE token = @Token";
                            MySqlCommand tokenCmd = new MySqlCommand(updateTokenQuery, conn, transaction);
                            tokenCmd.Parameters.AddWithValue("@Token", resetToken);
                            tokenCmd.ExecuteNonQuery();

                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    statusLabel.Text = "Error updating password. Please try again.";
                    return false;
                }
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
} 