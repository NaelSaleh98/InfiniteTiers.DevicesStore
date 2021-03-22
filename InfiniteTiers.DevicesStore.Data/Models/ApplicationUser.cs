using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;


namespace InfiniteTiers.DevicesStore.Data.Models
{
    /// <summary>
    /// Each User has User role or Operation Manager role can have 
    /// list of devices.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        #region Relationships
        public ICollection<Device> Devices { get; set; } 
        #endregion
    }
}
