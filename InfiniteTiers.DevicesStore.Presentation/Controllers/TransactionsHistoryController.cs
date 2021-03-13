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

        // GET: History
        public async Task<IActionResult> Index()
        {
            var history = _context.UserDevices
                        .Include(us => us.FromUser)
                        .Include(us => us.ToUser)
                        .Include(us => us.Device)
                        .OrderBy(us => us.TransactionDate);

            return View(await history.ToListAsync());
        }
    }
}
