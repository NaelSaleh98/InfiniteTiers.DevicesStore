using InfiniteTiers.DevicesStore.Presentation.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniteTiers.DevicesStore.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TransactionsHistoryController : Controller
    {
        private readonly AuthDbContext _context;

        public TransactionsHistoryController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: Devices
        public async Task<IActionResult> Index()
        {
            var devices = _context.Devices
                        .Include(d => d.Category)
                        .OrderBy(d => d.IsActive);

            return View(await devices.ToListAsync());
        }

        // GET: History
        public IActionResult History(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var history = from h in _context.UserDevices
                          .Include(us => us.FromUser)
                          .Include(us => us.ToUser)
                          .OrderByDescending(us => us.TransactionDate)
                          where h.Device.DeviceId == id
                          select h;

            if (history == null)
            {
                return NotFound();
            }
            var device = _context.Devices.FirstOrDefaultAsync(u => u.DeviceId == id);
            ViewData["Device"] = device.Result.Name;

            return View(history);
        }

    }
}
