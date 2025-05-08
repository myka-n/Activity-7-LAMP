using System;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;
using System.Net.Mail;
using System.Configuration;

namespace Activity_7
{
    public static class AuthService
    {
        private static readonly string EmailAddress = ConfigurationManager.AppSettings["EmailAddress"];
        private static readonly string EmailPassword = ConfigurationManager.AppSettings["EmailPassword"];
        private static readonly string ResetLinkBaseUrl = ConfigurationManager.AppSettings["ResetLinkBaseUrl"];

        public static User Login(string username, string password)
        {
            string query = "SELECT * FROM users WHERE username = @username AND is_active = 1";
            User user = null;

            DatabaseManager.ExecuteQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@username", username);
            }, reader =>
            {
                if (reader.Read() && VerifyPassword(password, reader.GetString("password_hash")))
                {
                    user = new User
                    {
                        UserId = reader.GetInt32("user_id"),
                        Username = reader.GetString("username"),
                        Email = reader.GetString("email"),
                        Role = reader.GetString("role"),
                        LastLogin = DateTime.Now
                    };

                    // Update last login
                    UpdateLastLogin(user.UserId);
                }
            });

            return user;
        }

        public static bool RequestPasswordReset(string email)
        {
            string token = GenerateResetToken();
            DateTime expiry = DateTime.Now.AddHours(24);

            string query = @"
                UPDATE users 
                SET reset_token = @token, 
                    reset_token_expiry = @expiry 
                WHERE email = @email";

            bool success = false;
            DatabaseManager.ExecuteQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@token", token);
                cmd.Parameters.AddWithValue("@expiry", expiry);
                cmd.Parameters.AddWithValue("@email", email);
                success = cmd.ExecuteNonQuery() > 0;
            }, null);

            if (success)
            {
                SendPasswordResetEmail(email, token);
            }

            return success;
        }

        public static bool ResetPassword(string token, string newPassword)
        {
            string query = @"
                UPDATE users 
                SET password_hash = @passwordHash,
                    reset_token = NULL,
                    reset_token_expiry = NULL
                WHERE reset_token = @token 
                AND reset_token_expiry > NOW()";

            bool success = false;
            DatabaseManager.ExecuteQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@passwordHash", HashPassword(newPassword));
                cmd.Parameters.AddWithValue("@token", token);
                success = cmd.ExecuteNonQuery() > 0;
            }, null);

            return success;
        }

        public static bool CreateUser(User user, string password)
        {
            string query = @"
                INSERT INTO users (
                    username, email, password_hash, role, 
                    created_at, is_active
                ) VALUES (
                    @username, @email, @passwordHash, @role,
                    @createdAt, 1
                )";

            bool success = false;
            DatabaseManager.ExecuteQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@passwordHash", HashPassword(password));
                cmd.Parameters.AddWithValue("@role", user.Role ?? "user");
                cmd.Parameters.AddWithValue("@createdAt", DateTime.Now);
                success = cmd.ExecuteNonQuery() > 0;
            }, null);

            return success;
        }

        public static bool UpdateUser(User user)
        {
            string query = @"
                UPDATE users 
                SET username = @username,
                    email = @email,
                    bio = @bio,
                    profile_pic = @profilePic,
                    role = @role,
                    is_active = @isActive
                WHERE user_id = @userId";

            bool success = false;
            DatabaseManager.ExecuteQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@bio", user.Bio);
                cmd.Parameters.AddWithValue("@profilePic", user.ProfilePic);
                cmd.Parameters.AddWithValue("@role", user.Role);
                cmd.Parameters.AddWithValue("@isActive", user.IsActive);
                cmd.Parameters.AddWithValue("@userId", user.UserId);
                success = cmd.ExecuteNonQuery() > 0;
            }, null);

            return success;
        }

        public static bool DeleteUser(int userId)
        {
            string query = "UPDATE users SET is_active = 0 WHERE user_id = @userId";
            bool success = false;
            DatabaseManager.ExecuteQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                success = cmd.ExecuteNonQuery() > 0;
            }, null);
            return success;
        }

        private static void UpdateLastLogin(int userId)
        {
            string query = "UPDATE users SET last_login = @lastLogin WHERE user_id = @userId";
            DatabaseManager.ExecuteQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@lastLogin", DateTime.Now);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.ExecuteNonQuery();
            }, null);
        }

        private static string GenerateResetToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        private static void SendPasswordResetEmail(string email, string token)
        {
            try
            {
                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.Credentials = new System.Net.NetworkCredential(EmailAddress, EmailPassword);

                    var message = new MailMessage
                    {
                        From = new MailAddress(EmailAddress),
                        Subject = "Password Reset Request",
                        Body = $"Click the following link to reset your password: {ResetLinkBaseUrl}?token={token}",
                        IsBodyHtml = false
                    };
                    message.To.Add(email);

                    client.Send(message);
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("Failed to send password reset email", ex);
            }
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private static bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
} 