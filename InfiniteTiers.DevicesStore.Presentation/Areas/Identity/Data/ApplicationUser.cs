using InfiniteTiers.DevicesStore.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniteTiers.DevicesStore.Presentation.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Device> Devices { get; set; }
    }
}
