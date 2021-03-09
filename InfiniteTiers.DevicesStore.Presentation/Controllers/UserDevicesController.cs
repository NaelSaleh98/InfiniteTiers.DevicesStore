using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InfiniteTiers.DevicesStore.Data.DAL;
using InfiniteTiers.DevicesStore.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace InfiniteTiers.DevicesStore.Presentation.Controllers
{
    [Authorize]

    public class UserDevicesController : Controller
    {
        private readonly ItgContext _context;

        public UserDevicesController(ItgContext context)
        {
            _context = context;
        }

        // GET: UserDevices
        public async Task<IActionResult> Index()
        {
            var userDevices = _context.UserDevices
                .Include(x => x.User)
                .Include(x => x.Device);
            return View(await userDevices.ToListAsync());
        }

        // GET: UserDevices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDevice = await _context.UserDevices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userDevice == null)
            {
                return NotFound();
            }

            return View(userDevice);
        }

        // GET: UserDevices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserDevices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartTime,EndTime")] UserDevice userDevice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userDevice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userDevice);
        }

        // GET: UserDevices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDevice = await _context.UserDevices.FindAsync(id);
            if (userDevice == null)
            {
                return NotFound();
            }
            return View(userDevice);
        }

        // POST: UserDevices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,EndTime")] UserDevice userDevice)
        {
            if (id != userDevice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userDevice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDeviceExists(userDevice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userDevice);
        }

        // GET: UserDevices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDevice = await _context.UserDevices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userDevice == null)
            {
                return NotFound();
            }

            return View(userDevice);
        }

        // POST: UserDevices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userDevice = await _context.UserDevices.FindAsync(id);
            _context.UserDevices.Remove(userDevice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserDeviceExists(int id)
        {
            return _context.UserDevices.Any(e => e.Id == id);
        }
    }
}
