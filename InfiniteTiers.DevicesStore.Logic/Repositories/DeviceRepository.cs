using InfiniteTiers.DevicesStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfiniteTiers.DevicesStore.Logic.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        #region private fields
        private readonly AuthDbContext _context;
        private readonly ILogger _logger;
        #endregion

        #region Constructor
        public DeviceRepository(AuthDbContext context, ILogger<DeviceRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        #region Public methods
        public IEnumerable<Device> GetAll()
        {
            var devices = _context.Devices
                        .Include(d => d.Category)
                        .Include(d => d.OwnedBy)
                        .OrderBy(d => d.IsActive);
            return devices;
        }

        public Device GetById(int? id)
        {
            var device = _context.Devices
                        .Include(d => d.Category)
                        .Include(d => d.OwnedBy)
                        .Single(m => m.DeviceId == id);
            return device;
        }

        public bool Save(Device device)
        {
            try
            {
                _context.Add(device);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public bool Update(Device device)
        {
            try
            {
                _context.Update(device);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var device = GetById(id);
                _context.Devices.Remove(device);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public bool IsExist(int id)
        {
            return _context.Devices.Any(e => e.DeviceId == id);
        } 
        #endregion
    }
}
