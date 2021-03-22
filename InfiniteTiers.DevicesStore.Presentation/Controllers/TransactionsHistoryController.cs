using InfiniteTiers.DevicesStore.Data.Models;
using InfiniteTiers.DevicesStore.Logic.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniteTiers.DevicesStore.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TransactionsHistoryController : Controller
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDeviceHistoryRepository _historyRepository;

        public TransactionsHistoryController(IDeviceRepository deviceRepository,IDeviceHistoryRepository historyRepository)
        {
            _deviceRepository = deviceRepository;
            _historyRepository = historyRepository;
        }

        // GET: Devices
        public IActionResult Index()
        {
            var devices = _deviceRepository.GetDevices();

            return View(devices.ToList());
        }

        // GET: History
        public IActionResult History(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var history = _historyRepository.GetById(id);

            if (history == null)
            {
                return NotFound();
            }
            var device = _deviceRepository.GetDevice(id);
            ViewData["Device"] = device.Name;

            return View(history);
        }

    }
}
