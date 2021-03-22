using InfiniteTiers.DevicesStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfiniteTiers.DevicesStore.Logic.Repositories
{
    public class DeviceHistoryRepository : IDeviceHistoryRepository
    {
        #region private fields
        private readonly AuthDbContext _context;
        private readonly ILogger _logger;
        #endregion

        #region Constructor
        public DeviceHistoryRepository(AuthDbContext context, ILogger<DeviceHistoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        #region Public methods
        public bool Save(UserDevice userDevice)
        {
            try
            {
                _context.UserDevices.Add(userDevice);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public IEnumerable<UserDevice> GetById(int? deviceId)
        {
            var history = _context.UserDevices
                        .Include(us => us.FromUser)
                        .Include(us => us.ToUser)
                        .Include(us => us.Device)
                        .Where(us => us.Device.DeviceId == deviceId)
                        .OrderByDescending(us => us.TransactionDate);
            return history;
        } 
        #endregion
    }
}
