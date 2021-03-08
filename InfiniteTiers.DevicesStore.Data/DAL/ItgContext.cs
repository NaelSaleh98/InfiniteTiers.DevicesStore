using InfiniteTiers.DevicesStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteTiers.DevicesStore.Data.DAL
{
    public class ItgContext : DbContext
    {
        public ItgContext(DbContextOptions<ItgContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
    }
}
