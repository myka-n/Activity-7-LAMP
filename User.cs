using System;

namespace Activity_7
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string ProfilePic { get; set; }
        public string Bio { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
    }
} 