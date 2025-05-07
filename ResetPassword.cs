using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Activity_7
{
    public partial class ResetPassword : Form
    {
        private string resetToken;

        public ResetPassword(string token)
        {
            InitializeComponent();
            this.resetToken = token;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = System.Drawing.Color.White;
        }

        private void InitializeComponent()
        {
            this.newPasswordLabel = new System.Windows.Forms.Label();
            this.confirmPasswordLabel = new System.Windows.Forms.Label();
            this.newPasswordTextBox = new System.Windows.Forms.TextBox();
            this.confirmPasswordTextBox = new System.Windows.Forms.TextBox();
            this.resetButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();

            // newPasswordLabel
            this.newPasswordLabel.AutoSize = true;
            this.newPasswordLabel.Location = new System.Drawing.Point(20, 20);
            this.newPasswordLabel.Size = new System.Drawing.Size(100, 15);
            this.newPasswordLabel.Text = "New Password:";

            // confirmPasswordLabel
            this.confirmPasswordLabel.AutoSize = true;
            this.confirmPasswordLabel.Location = new System.Drawing.Point(20, 60);
            this.confirmPasswordLabel.Size = new System.Drawing.Size(120, 15);
            this.confirmPasswordLabel.Text = "Confirm Password:";

            // newPasswordTextBox
            this.newPasswordTextBox.Location = new System.Drawing.Point(20, 40);
            this.newPasswordTextBox.Size = new System.Drawing.Size(250, 20);
            this.newPasswordTextBox.UseSystemPasswordChar = true;

            // confirmPasswordTextBox
            this.confirmPasswordTextBox.Location = new System.Drawing.Point(20, 80);
            this.confirmPasswordTextBox.Size = new System.Drawing.Size(250, 20);
            this.confirmPasswordTextBox.UseSystemPasswordChar = true;

            // resetButton
            this.resetButton.Location = new System.Drawing.Point(20, 120);
            this.resetButton.Size = new System.Drawing.Size(100, 30);
            this.resetButton.Text = "Reset Password";
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);

            // cancelButton
            this.cancelButton.Location = new System.Drawing.Point(170, 120);
            this.cancelButton.Size = new System.Drawing.Size(100, 30);
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);

            // ResetPassword
            this.ClientSize = new System.Drawing.Size(290, 170);
            this.Controls.Add(this.newPasswordLabel);
            this.Controls.Add(this.confirmPasswordLabel);
            this.Controls.Add(this.newPasswordTextBox);
            this.Controls.Add(this.confirmPasswordTextBox);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.cancelButton);
            this.Name = "ResetPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reset Password";
        }

        private System.Windows.Forms.Label newPasswordLabel;
        private System.Windows.Forms.Label confirmPasswordLabel;
        private System.Windows.Forms.TextBox newPasswordTextBox;
        private System.Windows.Forms.TextBox confirmPasswordTextBox;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button cancelButton;

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(newPasswordTextBox.Text) || string.IsNullOrEmpty(confirmPasswordTextBox.Text))
            {
                MessageBox.Show("Please enter both passwords.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (newPasswordTextBox.Text != confirmPasswordTextBox.Text)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connStr = "server=localhost;user=root;password=mykz;database=zeereal_artspace";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    
                    // First, verify the token and get the user_id
                    string verifyQuery = @"SELECT user_id FROM password_reset_tokens 
                                         WHERE token = @Token AND expiry_date > NOW() 
                                         AND used = 0";
                    
                    MySqlCommand verifyCmd = new MySqlCommand(verifyQuery, conn);
                    verifyCmd.Parameters.AddWithValue("@Token", resetToken);
                    
                    object userIdResult = verifyCmd.ExecuteScalar();
                    
                    if (userIdResult != null)
                    {
                        int userId = Convert.ToInt32(userIdResult);
                        string hashedPassword = HashPassword(newPasswordTextBox.Text);
                        
                        // Update the password
                        string updateQuery = "UPDATE users SET password = @Password WHERE user_id = @UserId";
                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@Password", hashedPassword);
                        updateCmd.Parameters.AddWithValue("@UserId", userId);
                        
                        if (updateCmd.ExecuteNonQuery() > 0)
                        {
                            // Mark the token as used
                            string markUsedQuery = "UPDATE password_reset_tokens SET used = 1 WHERE token = @Token";
                            MySqlCommand markUsedCmd = new MySqlCommand(markUsedQuery, conn);
                            markUsedCmd.Parameters.AddWithValue("@Token", resetToken);
                            markUsedCmd.ExecuteNonQuery();
                            
                            MessageBox.Show("Password has been reset successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid or expired reset token.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
} 