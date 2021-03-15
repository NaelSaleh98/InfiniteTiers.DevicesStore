﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InfiniteTiers.DevicesStore.Data.Models
{
    public class Device
    {
        public int DeviceId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }

        public string SerialNumber { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool IsActive { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string ApplicationUserId { get; set; }

        public string OwnedBy { get; set; }
    }
}
