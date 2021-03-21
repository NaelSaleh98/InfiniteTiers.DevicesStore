using System;
using System.ComponentModel.DataAnnotations;

namespace InfiniteTiers.DevicesStore.Data.Models
{
    public class Device
    {
        public int DeviceId { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        [StringLength(120)]
        public string Description { get; set; }

        [StringLength(30)]
        [Required]
        public string Manufacturer { get; set; }

        [StringLength(60)]
        public string Model { get; set; }

        [StringLength(120)]
        [Required]
        public string SerialNumber { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Purchase Date")]
        public DateTime PurchaseDate { get; set; }

        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser OwnedBy { get; set; }
    }
}
