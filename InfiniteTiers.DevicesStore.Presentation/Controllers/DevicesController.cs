using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InfiniteTiers.DevicesStore.Data.Models;
using Microsoft.AspNetCore.Authorization;
using InfiniteTiers.DevicesStore.Presentation.Data;

namespace InfiniteTiers.DevicesStore.Presentation.Controllers
{
    [Authorize(Roles ="Admin")]
    public class DevicesController : Controller
    {
        private readonly AuthDbContext _context;
        public DevicesController(AuthDbContext context)
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

        // GET: Devices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .Include(d => d.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DeviceId == id);
            if (device == null)
            {
                return NotFound();
            }

            var user = _context.Users.FirstOrDefaultAsync(u => u.Id == device.ApplicationUserId);
            ViewData["User"] = user.Result.UserName;

            return View(device);
        }

        // GET: Devices/Create
        public IActionResult Create()
        {
            PopulateCategoriesDropDownList();
            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Manufacturer,Model,SerialNumber,PurchaseDate,CategoryId")] Device device)
        {
            if (ModelState.IsValid)
            {
                var role = _context.Roles.FirstOrDefaultAsync(r => r.Name == "Operation Manager");
                var roleUser = _context.UserRoles.FirstOrDefaultAsync(ur => ur.RoleId == role.Result.Id);
                device.ApplicationUserId = roleUser.Result.UserId;
                device.IsActive = false;
                _context.Add(device);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateCategoriesDropDownList(device.CategoryId);
            return View(device);
        }

        // GET: Devices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                                 .AsNoTracking()
                                .FirstOrDefaultAsync(d => d.DeviceId == id);
            if (device == null)
            {
                return NotFound();
            }
            PopulateCategoriesDropDownList(device.CategoryId);
            return View(device);
        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deviceToUpdate = await _context.Devices
                                .FirstOrDefaultAsync(d => d.DeviceId == id);

            if (await TryUpdateModelAsync<Device>(deviceToUpdate, "",
                d => d.Name, d => d.Description, d => d.Manufacturer,
                d => d.Model, d => d.SerialNumber, d => d.PurchaseDate,
                d => d.IsActive, d => d.CategoryId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateCategoriesDropDownList(deviceToUpdate.CategoryId);
            return View(deviceToUpdate);
        }

        // GET: Devices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .Include(d => d.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DeviceId == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.DeviceId == id);
        }

        private void PopulateCategoriesDropDownList(object selectedCategory = null)
        {
            var categoriesQuery = from c in _context.Categories
                                   orderby c.Name
                                   select c;
            ViewBag.CategoryId = new SelectList(categoriesQuery.AsNoTracking(), "CategoryId", "Name", selectedCategory);
        }

    }
}
