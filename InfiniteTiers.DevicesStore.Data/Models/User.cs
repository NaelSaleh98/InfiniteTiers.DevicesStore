using System;
using System.Collections.Generic;
using System.Text;

namespace InfiniteTiers.DevicesStore.Data.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }

        public ICollection<Device> Devices { get; set; }
    }
}
