using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Activity_7
{
    public static class Session
    {
        public static int UserId { get; set; }
        public static string Username { get; set; }
        public static string Email { get; set; }
        public static string ProfilePicPath { get; set; }
        public static string Bio { get; set; } 
        public static bool RememberMe { get; set; }

        internal static void Clear()
        {
            throw new NotImplementedException();
        }
    }
}

