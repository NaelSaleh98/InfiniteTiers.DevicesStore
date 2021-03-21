using InfiniteTiers.DevicesStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteTiers.DevicesStore.Logic.Repositories
{
    public class DeviceHistoryRepository : IDeviceHistoryRepository
    {
        private readonly AuthDbContext _context;

        public DeviceHistoryRepository(AuthDbContext context)
        {
            _context = context;
        }

        public void SaveDeviceHistory(UserDevice userDevice)
        {
            _context.UserDevices.Add(userDevice);
            _context.SaveChanges();
        }
    }
}
