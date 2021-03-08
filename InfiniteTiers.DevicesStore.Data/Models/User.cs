using System;
using System.Collections.Generic;
using System.Text;

namespace InfiniteTiers.DevicesStore.Data.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Role Role { get; set; }
        public ICollection<Device> Devices { get; set; }
    }
}
