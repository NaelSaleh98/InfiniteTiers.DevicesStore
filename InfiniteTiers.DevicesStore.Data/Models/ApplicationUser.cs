using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;


namespace InfiniteTiers.DevicesStore.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Device> Devices { get; set; }
    }
}
