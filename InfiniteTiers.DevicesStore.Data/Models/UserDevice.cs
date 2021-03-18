using System;
using System.ComponentModel.DataAnnotations;


namespace InfiniteTiers.DevicesStore.Data.Models
{
    public class UserDevice
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }

        public ApplicationUser FromUser { get; set; }

        public ApplicationUser ToUser { get; set; }

        public Device Device { get; set; }
    }
}
