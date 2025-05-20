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
using System.Configuration;

namespace Activity_7
{
   
    public partial class Login : Form
    {
        private bool isPasswordVisible = false;
        private const string RememberMeFile = "rememberMe.txt"; // Constant for the filename
        // AES Key and IV.  These should be stored securely, NOT hardcoded.
        private static readonly byte[] Key = Convert.FromBase64String(ConfigurationManager.AppSettings["EncryptionKey"]);
        private static readonly byte[] IV = Convert.FromBase64String(ConfigurationManager.AppSettings["EncryptionIV"]);


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

            // Check if we have a reset token in the command line arguments
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && args[1].StartsWith("token="))
            {
                string token = args[1].Substring(6); // Remove "token=" prefix
                OpenResetPasswordForm(token);
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

        private void OpenResetPasswordForm(string token)
        {
            try
            {
                ResetPassword resetForm = new ResetPassword(token);
                if (resetForm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Password has been reset successfully. Please login with your new password.", 
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                LogError("Error opening reset form", ex);
                MessageBox.Show("Error opening password reset form. Please try again.", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitiatePasswordReset(string userEmail)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
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
                        // Generate a 6-digit code
                        string resetCode = GenerateResetCode();
                        DateTime expirationTime = DateTime.UtcNow.AddHours(24);

                        if (StoreResetToken(userId, resetCode, expirationTime))
                        {
                            string emailBody = $@"
                            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;'>
                                <h2 style='color: #333;'>Password Reset Code</h2>
                                <p>Hello,</p>
                                <p>We received a request to reset your password for your Zeereal Artspace account.</p>
                                <p>Your password reset code is:</p>
                                <div style='text-align: center; margin: 30px 0;'>
                                    <h1 style='color: #4CAF50; font-size: 32px; letter-spacing: 5px;'>{resetCode}</h1>
                                </div>
                                <p>Enter this code in the password reset form to set a new password.</p>
                                <p>This code will expire in 24 hours.</p>
                                <hr style='margin: 20px 0;'>
                                <p style='color: #666; font-size: 12px;'>This is an automated message from Zeereal Artspace. Please do not reply to this email.</p>
                            </div>";

                            SendEmail(userEmail, "Password Reset Code", emailBody);
                            ShowResetCodeForm();
                        }
                        else
                        {
                            MessageBox.Show("Failed to generate reset code. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Email address not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (MySqlException ex)
                {
                    LogError("Database Error", ex);
                    MessageBox.Show("An error occurred while processing your request. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        private string GenerateResetCode()
        {
            // Generate a 6-digit code
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private void ShowResetCodeForm()
        {
            using (var form = new Form())
            {
                form.Text = "Enter Reset Code";
                form.Size = new Size(400, 200);
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MaximizeBox = false;
                form.MinimizeBox = false;

                var label = new Label
                {
                    Text = "Please enter the 6-digit code sent to your email:",
                    Location = new Point(20, 20),
                    Size = new Size(340, 40),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                var codeBox = new TextBox
                {
                    Location = new Point(20, 70),
                    Size = new Size(340, 25),
                    MaxLength = 6,
                    Font = new Font("Arial", 14),
                    TextAlign = HorizontalAlignment.Center
                };

                var submitButton = new Button
                {
                    Text = "Submit",
                    Location = new Point(20, 110),
                    Size = new Size(340, 35),
                    BackColor = Color.FromArgb(0, 122, 204),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Arial", 10, FontStyle.Bold)
                };

                submitButton.Click += (s, e) =>
                {
                    if (ValidateResetCode(codeBox.Text))
                    {
                        form.DialogResult = DialogResult.OK;
                        form.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid or expired code. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                form.Controls.AddRange(new Control[] { label, codeBox, submitButton });
                form.AcceptButton = submitButton;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Show the reset password form
                    using (var resetForm = new ResetPassword(codeBox.Text))
                    {
                        if (resetForm.ShowDialog() == DialogResult.OK)
                        {
                            MessageBox.Show("Password has been reset successfully. Please login with your new password.", 
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private bool ValidateResetCode(string code)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT user_id FROM password_reset_tokens 
                                   WHERE token = @Code AND expiry_date > NOW() 
                                   AND used = 0";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Code", code);
                    return cmd.ExecuteScalar() != null;
                }
                catch (Exception ex)
                {
                    LogError("Error validating reset code", ex);
                    return false;
                }
            }
        }

        private bool StoreResetToken(int userId, string token, DateTime expirationTime)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    // First, invalidate any existing tokens for this user
                    string invalidateQuery = "UPDATE password_reset_tokens SET used = 1 WHERE user_id = @UserId AND used = 0";
                    MySqlCommand invalidateCmd = new MySqlCommand(invalidateQuery, conn);
                    invalidateCmd.Parameters.AddWithValue("@UserId", userId);
                    invalidateCmd.ExecuteNonQuery();

                    // Insert the new token
                    string insertQuery = @"INSERT INTO password_reset_tokens (user_id, token, expiry_date, used) 
                                         VALUES (@UserId, @Token, @ExpiryDate, 0)";
                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@UserId", userId);
                    insertCmd.Parameters.AddWithValue("@Token", token);
                    insertCmd.Parameters.AddWithValue("@ExpiryDate", expirationTime);
                    
                    return insertCmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    LogError("Error storing reset token", ex);
                    return false;
                }
            }
        }

        private void SendEmail(string recipientEmail, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Zeereal Artspace", ConfigurationManager.AppSettings["EmailAddress"]));
                message.To.Add(new MailboxAddress("", recipientEmail));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = body;
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.Authenticate(ConfigurationManager.AppSettings["EmailAddress"], 
                                     ConfigurationManager.AppSettings["EmailPassword"]);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                LogError("Email Error", ex);
                string errorMessage = "Failed to send password reset email. ";
                if (ex.Message.Contains("Authentication"))
                {
                    errorMessage += "Please contact support for assistance.";
                }
                else
                {
                    errorMessage += "Please try again later.";
                }
                MessageBox.Show(errorMessage, "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LogError(string context, Exception ex)
        {
            string logMessage = $"[{DateTime.Now}] {context}: {ex.Message}\nStack Trace: {ex.StackTrace}\n";
            File.AppendAllText("error.log", logMessage);
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
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT user_id, username, email, password, profile_pic, bio, role FROM users WHERE email = @Email";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", textBox1.Text); // Email textbox
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string dbHashedPassword = reader["password"].ToString();
                        string enteredPassword = textBox2.Text; // Password textbox
                        bool rememberMe = checkBox1.Checked; // Get the state of the checkbox

                        // Store all user data before closing reader
                        int userId = reader.GetInt32("user_id");
                        string role = reader.GetString("role");
                        string username = reader.GetString("username");
                        string email = reader["email"].ToString();
                        string profilePic = reader["profile_pic"].ToString();
                        string bio = reader["bio"].ToString();
                        
                        // Close the reader before executing additional commands
                        reader.Close();

                        // Set session variables with stored values
                        Session.UserId = userId;
                        Session.Username = username;
                        Session.Email = email;
                        Session.ProfilePicPath = profilePic;
                        Session.Bio = bio;
                        Session.Role = role;
                        Session.RememberMe = rememberMe;

                        // Hash the entered password and compare it to the stored hash
                        string enteredHashedPassword = HashPassword(enteredPassword);

                        if (enteredHashedPassword == dbHashedPassword)
                        {
                            // Debug information
                            Console.WriteLine($"Logging in user: ID={userId}, Username={username}, Role={role}");

                            // Update last login
                            string updateQuery = "UPDATE users SET last_login = @LastLogin WHERE user_id = @UserId";
                            MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                            updateCmd.Parameters.AddWithValue("@LastLogin", DateTime.Now);
                            updateCmd.Parameters.AddWithValue("@UserId", userId);
                            updateCmd.ExecuteNonQuery();

                            // Log the login activity
                            string activityQuery = @"
                                INSERT INTO user_activity_logs 
                                (user_id, activity_type, timestamp) 
                                VALUES 
                                (@UserId, 'LOGIN', @Timestamp)";
                            MySqlCommand activityCmd = new MySqlCommand(activityQuery, conn);
                            activityCmd.Parameters.AddWithValue("@UserId", userId);
                            activityCmd.Parameters.AddWithValue("@Timestamp", DateTime.Now);
                            activityCmd.ExecuteNonQuery();

                            // Open appropriate form based on role
                            if (role.ToLower() == "admin")
                            {
                                MessageBox.Show($"Logging in as admin: ID={userId}, Username={username}", "Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                var adminDashboard = new AdminDashboard(userId);
                                adminDashboard.Show();
                                this.Hide();
                            }
                            else
                            {
                                var homepage = new Homepage();
                                homepage.Show();
                                this.Hide();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Incorrect password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Email not found or account is inactive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
