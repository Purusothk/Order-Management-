using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.models
{
    internal class Users
    {
        public int UserId { get; set; } // Assuming this is needed for database operations
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        // Constructor
        public Users(string username, string password, string role)
        {
            Username = username;
            Password = password;
            Role = role;
        }
        //author :Purusothaman
        public Users() { }

    
    }
}
