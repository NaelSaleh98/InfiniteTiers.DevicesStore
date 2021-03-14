using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InfiniteTiers.DevicesStore.Data.Models
{
    public class Device
    {
        public int DeviceId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }

        [Required]
        public string SerialNumber { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool IsActive { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public Category Category { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
