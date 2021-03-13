using InfiniteTiers.DevicesStore.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniteTiers.DevicesStore.Presentation.Areas.Identity.Data
{
    public class UserDevice
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TransactionDate { get; set; }

        public ApplicationUser FromUser { get; set; }

        public ApplicationUser ToUser { get; set; }

        public Device Device { get; set; }
    }
}
