using System;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Activity_7
{
    public partial class ResetPasswordForm : Form
    {
        private string _resetToken;

        public ResetPasswordForm(string token)
        {
            InitializeComponent();
            _resetToken = token;
        }

        private void ResetPasswordForm_Load(object sender, EventArgs e)
        {
            // Validate the token and check for expiration
            if (!ValidateResetToken(_resetToken))
            {
                MessageBox.Show("Invalid or expired password reset token.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private bool ValidateResetToken(string token)
        {
            string connStr = "server=localhost;user=root;password=mykz;database=zeereal_artspace";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT user_id FROM password_reset_tokens WHERE token = @Token AND expiry_date > @Now";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Token", token);
                    cmd.Parameters.AddWithValue("@Now", DateTime.UtcNow);
                    object userIdResult = cmd.ExecuteScalar();
                    return userIdResult != null;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Database Error: " + ex.Message);
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            string newPassword = newPasswordTextBox.Text;
            string confirmPassword = confirmPasswordTextBox.Text;

            // Password validation
            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please enter and confirm your new password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword.Length < 6) // Example minimum length
            {
                MessageBox.Show("Password must be at least 6 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Update password in the database
            if (UpdatePassword(newPassword))
            {
                MessageBox.Show("Password reset successful! You can now log in with your new password.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Optionally, redirect the user back to the login form
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to reset password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool UpdatePassword(string newPassword)
        {
            string connStr = "server=localhost;user=root;password=mykz;database=zeereal_artspace";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    // First, get the user_id associated with the token
                    string selectUserIdQuery = "SELECT user_id FROM password_reset_tokens WHERE token = @Token";
                    MySqlCommand selectCmd = new MySqlCommand(selectUserIdQuery, conn);
                    selectCmd.Parameters.AddWithValue("@Token", _resetToken);
                    object userIdResult = selectCmd.ExecuteScalar();

                    if (userIdResult != null)
                    {
                        int userId = Convert.ToInt32(userIdResult);
                        string hashedPassword = HashPassword(newPassword);
                        string updateQuery = "UPDATE users SET password = @Password WHERE user_id = @UserId";
                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@Password", hashedPassword);
                        updateCmd.Parameters.AddWithValue("@UserId", userId);

                        if (updateCmd.ExecuteNonQuery() > 0)
                        {
                            // Delete the used reset token
                            string deleteTokenQuery = "DELETE FROM password_reset_tokens WHERE token = @Token";
                            MySqlCommand deleteCmd = new MySqlCommand(deleteTokenQuery, conn);
                            deleteCmd.Parameters.AddWithValue("@Token", _resetToken);
                            deleteCmd.ExecuteNonQuery();
                            return true;
                        }
                    }
                    return false;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Database Error: " + ex.Message);
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
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