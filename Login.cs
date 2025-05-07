using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.IO;
using System.IO.IsolatedStorage;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Activity_7
{
   
    public partial class Login : Form
    {
        private bool isPasswordVisible = false;
        private const string RememberMeFile = "rememberMe.txt"; // Constant for the filename
        // AES Key and IV.  These should be stored securely, NOT hardcoded.
        private static readonly byte[] Key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 }; // Example, REPLACE
        private static readonly byte[] IV = { 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F, 0x20 };    // Example, REPLACE


        public Login()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White;
            SetRoundedRegion(20);
            InitializePasswordToggle();
            LoadRememberedLogin(); // Load saved login info on startup
            if (forgotPasswordLink != null) // Assuming you've added a LinkLabel named 'forgotPasswordLink' in your designer
            {
                forgotPasswordLink.LinkClicked += ForgotPasswordLink_LinkClicked;
            }
        }

        private void InitializePasswordToggle()
        {
            if (togglePasswordButton != null)
            {
                togglePasswordButton.FlatStyle = FlatStyle.Flat;
                togglePasswordButton.FlatAppearance.BorderSize = 0;
                togglePasswordButton.BackColor = Color.White; // Match the textbox background
                togglePasswordButton.ForeColor = Color.Black; // Or a color that contrasts
                togglePasswordButton.Font = new Font("Webdings", 10); // Use a font with an eye-like character
                togglePasswordButton.Text = "\u0065"; // The "v" character in Webdings often looks like an eye
                togglePasswordButton.Size = new Size(30, 23); // Adjust size as needed in the designer
                togglePasswordButton.Location = new Point(textBox2.Location.X + textBox2.Width - togglePasswordButton.Width - 5, textBox2.Location.Y + (textBox2.Height - togglePasswordButton.Height) / 2);
                togglePasswordButton.Cursor = Cursors.Hand;
                togglePasswordButton.Click += TogglePasswordVisibility_Click; // Ensure this is wired up
            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetRoundedRegion(30); // keeps it rounded on resize
                                  // Adjust the toggle button position on resize
            if (togglePasswordButton != null)
            {
                togglePasswordButton.Location = new Point(textBox2.Location.X + textBox2.Width - togglePasswordButton.Width - 5, textBox2.Location.Y + (textBox2.Height - togglePasswordButton.Height) / 2);
            }
        }

        private void TogglePasswordVisibility_Click(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            textBox2.UseSystemPasswordChar = !isPasswordVisible;

            if (isPasswordVisible)
            {
                togglePasswordButton.Text = "\u0066"; // Open eye (show password)
            }
            else
            {
                togglePasswordButton.Text = "\u0065"; // Closed eye (hide password)
            }
        }
            
        private void ForgotPasswordLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Open a dialog or navigate to a new form to get the user's email for password reset
            string userEmail = ShowForgotPasswordDialog();
            if (!string.IsNullOrEmpty(userEmail))
            {
                InitiatePasswordReset(userEmail);
            }
        }

        private string ShowForgotPasswordDialog()
        {
            string email = "";
            using (var dialog = new Form { Text = "Forgot Password", StartPosition = FormStartPosition.CenterParent, Width = 300, Height = 150 })
            {
                var emailLabel = new Label { Text = "Enter your email:", Location = new Point(20, 20), Size = new Size(260, 15) };
                var emailTextBox = new TextBox { Location = new Point(20, 40), Size = new Size(260, 20) };
                var resetButton = new Button { Text = "Reset Password", Location = new Point(100, 80), Size = new Size(100, 30) };
                var cancelButton = new Button { Text = "Cancel", Location = new Point(200, 80), Size = new Size(80, 30) };

                dialog.Controls.Add(emailLabel);
                dialog.Controls.Add(emailTextBox);
                dialog.Controls.Add(resetButton);
                dialog.Controls.Add(cancelButton);

                resetButton.Click += (s, ea) => { email = emailTextBox.Text; dialog.DialogResult = DialogResult.OK; dialog.Close(); };
                cancelButton.Click += (s, ea) => { dialog.DialogResult = DialogResult.Cancel; dialog.Close(); };

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return email;
                }
            }
            return null;
        }

        private void InitiatePasswordReset(string userEmail)
        {
            // 1. Check if the email exists in the database
            string connStr = "server=localhost;user=root;password=mykz;database=zeereal_artspace";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT user_id FROM users WHERE email = @Email";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", userEmail);
                    object userIdResult = cmd.ExecuteScalar();

                    if (userIdResult != null)
                    {
                        int userId = Convert.ToInt32(userIdResult);
                        // 2. Generate a secure reset token
                        string resetToken = GenerateResetToken();
                        DateTime expirationTime = DateTime.UtcNow.AddHours(24); // Token expires in 24 hours

                        // 3. Store the token, user ID, and expiration time in the database
                        if (StoreResetToken(userId, resetToken, expirationTime))
                        {
                            // Construct the reset link with the token as a query parameter
                            string resetLink = $"http://localhost/reset?token={resetToken}"; // Changed to a proper URL format

                            string emailBody = $@"
                            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;'>
                                <h2 style='color: #333;'>Password Reset Request</h2>
                                <p>Hello,</p>
                                <p>We received a request to reset your password for your Zeereal Artspace account.</p>
                                <p>Click the button below to reset your password:</p>
                                <div style='text-align: center; margin: 30px 0;'>
                                    <a href='{resetLink}' style='background-color: #4CAF50; color: white; padding: 12px 24px; text-decoration: none; border-radius: 4px; display: inline-block;'>Reset Password</a>
                                </div>
                                <p>If you didn't request this password reset, you can safely ignore this email.</p>
                                <p>This link will expire in 24 hours.</p>
                                <hr style='margin: 20px 0;'>
                                <p style='color: #666; font-size: 12px;'>This is an automated message from Zeereal Artspace. Please do not reply to this email.</p>
                            </div>";

                            SendEmail(userEmail, "Password Reset Request", emailBody);

                            MessageBox.Show("A password reset link has been sent to your email address.", "Password Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Email address not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        private string GenerateResetToken()
        {
            // Generate a cryptographically secure random token
            byte[] tokenBytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(tokenBytes);
                return Convert.ToBase64String(tokenBytes);
            }
        }

        private bool StoreResetToken(int userId, string resetToken, DateTime expirationTime)
        {
            string connStr = "server=localhost;user=root;password=mykz;database=zeereal_artspace";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO password_reset_tokens (user_id, token, expiry_date) VALUES (@UserId, @Token, @Expiry)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Token", resetToken);
                    cmd.Parameters.AddWithValue("@Expiry", expirationTime);
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error storing reset token: " + ex.Message);
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        private void SendEmail(string recipientEmail, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Zeereal Artspace", "zeereal.artspace@gmail.com")); // Replace with your actual email
                message.To.Add(new MailboxAddress("", recipientEmail));
                message.Subject = subject;

                // Create a more professional HTML email body
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = body;

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Gmail SMTP settings
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    
                    // Replace these with your actual credentials
                    client.Authenticate("mykanaparato@gmail.com", "ewfmyzzcrvznsblo");

                    client.Send(message);
                    client.Disconnect(true);
                }

                MessageBox.Show("Password reset instructions have been sent to your email.", "Email Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string errorMessage = "Failed to send password reset email. ";
                if (ex.Message.Contains("Authentication"))
                {
                    errorMessage += "Please contact support for assistance.";
                }
                else
                {
                    errorMessage += ex.Message;
                }
                MessageBox.Show(errorMessage, "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SetRoundedRegion(int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new Rectangle(this.Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(this.Width - radius, this.Height - radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, this.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();

            this.Region = new Region(path);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // No functionality
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // No functionality
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            SignUp signUpForm = new SignUp();
            signUpForm.ShowDialog();
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

        private void button1_Click(object sender, EventArgs e)
        {
            // Database connection
            string connStr = "server=localhost;user=root;password=mykz;database=zeereal_artspace";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT user_id, username, email, password, profile_pic, bio FROM users WHERE email = @Email";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", textBox1.Text); // Email textbox
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string dbHashedPassword = reader["password"].ToString();
                        string enteredPassword = textBox2.Text; // Password textbox
                        bool rememberMe = checkBox1.Checked; // Get the state of the checkbox

                        // Hash the entered password and compare it to the stored hash
                        string enteredHashedPassword = HashPassword(enteredPassword);

                        if (enteredHashedPassword == dbHashedPassword)
                        {
                            // ⭐ SET THE SESSION VALUES
                            Session.UserId = Convert.ToInt32(reader["user_id"]);
                            Session.Username = reader["username"].ToString();
                            Session.Email = reader["email"].ToString();
                            Session.ProfilePicPath = reader["profile_pic"] != DBNull.Value ? reader["profile_pic"].ToString() : "";
                            Session.Bio = reader["bio"].ToString();
                            Session.RememberMe = rememberMe; // Store RememberMe state

                            // Save login information if "Remember Me" is checked
                            if (rememberMe)
                            {
                                SaveRememberedLogin(textBox1.Text, enteredPassword);
                            }
                            else
                            {
                                ClearRememberedLogin();
                            }

                            // Successful login
                            MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Hide(); // Hide login form
                            Homepage homepageForm = new Homepage();
                            homepageForm.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Incorrect password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Email not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    reader.Close(); // Close the reader.  Important!
                }
                catch (MySqlException ex)
                {
                    // More specific error handling for database errors.
                    MessageBox.Show("Database Error: " + ex.Message + "\n\nError Code: " + ex.Number, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    // General error handling (e.g., connection issues, other unexpected errors)
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Ensure the connection is closed, even if an error occurs.
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // No functionality
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // No functionality
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // No functionality
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // No functionality
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button2.BackColor = Color.Transparent;
            button2.ForeColor = Color.Black; // or White
            button2.Font = new Font("Arial", 10, FontStyle.Bold);
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // No functionality
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {

        }

        private void togglePasswordButton_Click(object sender, EventArgs e)
        {

        }

        // --- "Remember Me" Implementation ---
        private void SaveRememberedLogin(string email, string password)
        {
            try
            {
                // Use Isolated Storage for user-specific data
                using (var storage = IsolatedStorageFile.GetUserStoreForAssembly())
                {
                    using (var stream = new IsolatedStorageFileStream(RememberMeFile, FileMode.Create, storage))
                    {
                        using (var writer = new StreamWriter(stream))
                        {
                            //  IMPORTANT:  ENCRYPT the password before saving it.
                            string encryptedPassword = EncryptString(password);
                            if (encryptedPassword != null) // Check if encryption was successful
                            {
                                writer.WriteLine(email);
                                writer.WriteLine(encryptedPassword);
                            }
                            else
                            {
                                // Handle the error:  Consider showing a message to the user
                                Console.WriteLine("Failed to encrypt password.  Not saving Remember Me.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors during saving (e.g., storage full, permissions).
                Console.WriteLine("Error saving login info: " + ex.Message);
            }
        }

        private void LoadRememberedLogin()
        {
            try
            {
                using (var storage = IsolatedStorageFile.GetUserStoreForAssembly())
                {
                    if (storage.FileExists(RememberMeFile))
                    {
                        using (var stream = new IsolatedStorageFileStream(RememberMeFile, FileMode.Open, storage))
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                string email = reader.ReadLine();
                                string encryptedPassword = reader.ReadLine();
                                string password = DecryptString(encryptedPassword); // Decrypt the password
                                if (password != null) // Check if decryption was successful
                                {
                                    textBox1.Text = email;
                                    textBox2.Text = password;
                                    checkBox1.Checked = true; // Check the "Remember Me" box
                                }
                                else
                                {
                                    // Handle error, e.g., clear checkbox, show message
                                    checkBox1.Checked = false;
                                    Console.WriteLine("Failed to decrypt password.  Not loading Remember Me.");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors during loading.
                Console.WriteLine("Error loading saved login: " + ex.Message);
            }
        }

        private void ClearRememberedLogin()
        {
            try
            {
                using (var storage = IsolatedStorageFile.GetUserStoreForAssembly())
                {
                    if (storage.FileExists(RememberMeFile))
                    {
                        storage.DeleteFile(RememberMeFile);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors during clearing.
                Console.WriteLine("Error clearing saved login: " + ex.Message);
            }
        }



        // --- Encryption Helper Methods (AES Example) ---
        //  IMPORTANT:  Replace these with your actual, secure encryption implementation.
        //  This is a *basic* example and may not be suitable for production use without
        //  careful review and security auditing.  Use a strong, well-vetted encryption library.

        // private const string Key = "YourSecureKey"; //  Replace with a strong, *random*, and hardcoded key.
        // private const string IV = "YourSecureIV";    //  Replace with a strong, *random*, and hardcoded IV.
        //  The Key and IV should NOT be hardcoded as strings.  They should be byte arrays.
        //  And, they should be derived from a secure source, not directly hardcoded.

        private string EncryptString(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return null; // Or throw an ArgumentNullException

            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;
                    aesAlg.Padding = PaddingMode.PKCS7; // Explicitly set padding

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle encryption errors.  Log them!  Return null or an empty string to indicate failure.
                Console.WriteLine("Encryption Error: " + ex.Message);
                return null;
            }
        }

        private string DecryptString(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return null; // Or throw an ArgumentNullException

            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;
                    aesAlg.Padding = PaddingMode.PKCS7;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle decryption errors. Log the error and return a default value or rethrow.
                Console.WriteLine("Decryption Error: " + ex.Message);
                return null; // Return null to indicate failure
            }
        }
    }
}
