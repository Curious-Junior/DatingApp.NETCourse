using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class AppUser
    {
        // These two should be name exactly as entity framework and ASP.NET CORE identity
        // do a lot of manual stuff automatically if you name them a certain name
        // The user id number should be named Id or ID
        public int Id { get; set; }
        // The user name should be named UserName every time
        public string UserName { get; set; }

        // the password hash and password salt
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}