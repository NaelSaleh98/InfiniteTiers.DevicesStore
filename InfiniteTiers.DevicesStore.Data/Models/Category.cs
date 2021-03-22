using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InfiniteTiers.DevicesStore.Data.Models
{
    /// <summary>
    /// The category can have name and list of devices belonging to it.
    /// </summary>
    public class Category
    {
        #region Basic
        public int CategoryId { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }
        #endregion

        #region Relationships
        public ICollection<Device> Devices { get; set; }
        #endregion    
    }
}
