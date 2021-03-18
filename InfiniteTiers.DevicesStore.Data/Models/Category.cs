using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InfiniteTiers.DevicesStore.Data.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        public ICollection<Device> Devices { get; set; }
    }
}
