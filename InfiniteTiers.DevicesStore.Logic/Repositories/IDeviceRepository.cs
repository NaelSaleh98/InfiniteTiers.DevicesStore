using InfiniteTiers.DevicesStore.Data.Models;
using System.Collections.Generic;

namespace InfiniteTiers.DevicesStore.Logic.Repositories
{
    public interface IDeviceRepository
    {
        /// <summary>
        /// Get all devices and the category and owner of each one.
        /// </summary>
        /// <returns>List of all devices ordered by status.</returns>
        public IEnumerable<Device> GetAll();

        /// <summary>
        /// Get specific device.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Device and its category and owner.</returns>
        public Device GetById(int? id);

        /// <summary>
        /// Save new device.
        /// </summary>
        /// <param name="device"></param>
        /// <returns>true if success, false if not.</returns>
        public bool Save(Device device);

        /// <summary>
        /// Update exist device.
        /// </summary>
        /// <param name="device"></param>
        /// <returns>true if success, false if not.</returns>
        public bool Update(Device device);

        /// <summary>
        /// Delete exist Device.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if success, false if not.</returns>
        public bool Delete(int id);

        /// <summary>
        /// Determine if device exist or not.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if exist, false if not.</returns>
        public bool IsExist(int id);

    }
}
