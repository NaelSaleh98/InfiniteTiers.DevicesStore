using InfiniteTiers.DevicesStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteTiers.DevicesStore.Logic.Repositories
{
    public interface IDeviceRepository
    {
        public IEnumerable<Device> GetDevices();
        public Device GetDevice(int? id);
        public void SaveDevice(Device device);
        public void UpdateDevice(Device device);
        public void DeleteDevice(int id);
        public bool DeviceExists(int id);

    }
}
