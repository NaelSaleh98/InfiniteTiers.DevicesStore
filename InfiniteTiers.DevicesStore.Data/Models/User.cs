﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InfiniteTiers.DevicesStore.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
    }
}