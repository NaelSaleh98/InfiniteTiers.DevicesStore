using InfiniteTiers.DevicesStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteTiers.DevicesStore.Logic.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly AuthDbContext _context;

        public DeviceRepository (AuthDbContext context)
        {
            _context = context;
        }


        public IEnumerable<Device> GetDevices()
        {
            var devices = _context.Devices
                            .Include(d => d.Category)
                            .Include(d => d.OwnedBy)
                            .OrderBy(d => d.IsActive);

            return devices.ToList();
        }

        public Device GetDevice(int? id)
        {
            var device =  _context.Devices
                .Include(d => d.Category)
                .Include(d => d.OwnedBy)
                .AsNoTracking()
                .FirstOrDefault(m => m.DeviceId == id);

            return device;
        }
        public void SaveDevice(Device device)
        {
            _context.Add(device);

            _context.SaveChanges();
        }
        public void UpdateDevice(Device device)
        {
            _context.Update(device);
            _context.SaveChanges();
        }

        public void DeleteDevice(int id)
        {
            var device = _context.Devices
                .Include(d => d.Category)
                .Include(d => d.OwnedBy)
                .Single(d => d.DeviceId == id);
            _context.Devices.Remove(device);
            _context.SaveChanges();
        }
        public bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.DeviceId == id);
        }
    }
}
