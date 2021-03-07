using System;
using System.Collections.Generic;
using System.Text;

namespace InfiniteTiers.DevicesStore.Data.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
