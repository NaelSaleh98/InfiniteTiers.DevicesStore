using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteTiers.DevicesStore.Data.Models
{
    public class CategoryDevice
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public int CategoryId { get; set; }

        public virtual Device Device { get; set; }
        public virtual Category Category { get; set; }
    }
}
