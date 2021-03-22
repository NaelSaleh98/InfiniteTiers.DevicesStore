using InfiniteTiers.DevicesStore.Data.Models;
using System.Collections.Generic;

namespace InfiniteTiers.DevicesStore.Logic.Repositories
{
    public interface IDeviceHistoryRepository
    {
        /// <summary>
        /// save new history record.
        /// </summary>
        /// <param name="userDevice"></param>
        /// <returns>true if success, false if failed.</returns>
        public bool Save(UserDevice userDevice);

        /// <summary>
        /// Get device history by device id.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns>All device transactions.</returns>
        public IEnumerable<UserDevice> GetById(int? deviceId);
    }
}
