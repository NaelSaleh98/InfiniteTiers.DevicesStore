using InfiniteTiers.DevicesStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteTiers.DevicesStore.Logic.Repositories
{
    public interface IDeviceHistoryRepository
    {
        public void SaveDeviceHistory(UserDevice userDevice);

        public IEnumerable<UserDevice> GetDeviceHistory(int? deviceId);
    }
}
