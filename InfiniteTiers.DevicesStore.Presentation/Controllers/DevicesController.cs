using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InfiniteTiers.DevicesStore.Data.Models;
using Microsoft.AspNetCore.Authorization;
using InfiniteTiers.DevicesStore.Presentation.Services;
using InfiniteTiers.DevicesStore.Presentation.Models;

namespace InfiniteTiers.DevicesStore.Presentation.Controllers
{
    [Authorize]
    public class DevicesController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly IMailService _mailService;


        public DevicesController(AuthDbContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        // GET: Devices
        public async Task<IActionResult> Index(string searchString)
        {
            var devices = _context.Devices
                        .Include(d => d.Category)
                        .Include(d => d.OwnedBy)
                        .OrderBy(d => d.IsActive);
            if (!String.IsNullOrEmpty(searchString))
            {
                devices = (IOrderedQueryable<Device>)devices.Where(d => d.Name.Contains(searchString));
            }
            return View(await devices.ToListAsync());
        }

        [Authorize(Roles = "Admin,OperationManager")]
        // GET: Devices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .Include(d => d.Category)
                .Include(d => d.OwnedBy)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DeviceId == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        [Authorize(Roles = "Admin,OperationManager")]

        // GET: Devices/Create
        public IActionResult Create()
        {
            PopulateCategoriesDropDownList();
            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,OperationManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Manufacturer,Model,SerialNumber,PurchaseDate,CategoryId")] Device device)
        {

            if (ModelState.IsValid)
            {
                var role = _context.Roles.FirstOrDefaultAsync(r => r.Name == "OperationManager");
                var roleUser = _context.UserRoles.FirstOrDefaultAsync(ur => ur.RoleId == role.Result.Id);
                var user = _context.Users.FirstOrDefaultAsync(u => u.Id == roleUser.Result.UserId);
                device.ApplicationUserId = user.Result.Id;
                device.IsActive = false;
                _context.Add(device);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateCategoriesDropDownList(device.CategoryId);
            return View(device);
        }

        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "User,OperationManager")]
        // GET: Devices/request/5?userId=12
        public new async Task<IActionResult> Request(int? id, string userId)
        {
            if ( (id == null) || (userId ==null) )
            {
                return NotFound();
            }

            var device = await _context.Devices.FirstAsync(m => m.DeviceId == id);
            var ownedBy = await _context.Users.FirstAsync(ob => ob.Id == device.ApplicationUserId);
            var requester = await _context.Users.FirstAsync(r => r.Id == userId);

            if ( (device == null) || (ownedBy == null) || (requester == null) )
            {
                return NotFound();
            }

            MailRequest request = new MailRequest { To = "naels141@gmail.com", Subject = "Deivce Request"};
            request.PrepeareDeviceRequestBody(device, requester);
            await _mailService.SendEmailAsync(request);


            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "User,OperationManager")]
        // GET: Devices/Accept/5?userId=12
        public  async Task<IActionResult> Accept(int? id, string userId)
        {
            if ((id == null) || (userId == null))
            {
                return NotFound();
            }

            var device = await _context.Devices.FirstAsync(m => m.DeviceId == id);
            var ownedBy = await _context.Users.FirstAsync(ob => ob.Id == device.ApplicationUserId);
            var requester = await _context.Users.FirstAsync(r => r.Id == userId);



            if ((device == null) || (ownedBy == null) || (requester == null))
            {
                return NotFound();
            }


            device.ApplicationUserId = requester.Id;
            device.OwnedBy = requester;
            if (device.OwnedBy.UserName != "OperationManager")
            {
                device.IsActive = true;
            }
            else
            {
                device.IsActive = false;
            }

            await _context.SaveChangesAsync();

            MailRequest request = new MailRequest { To = "naels141@gmail.com", Subject = "Deivce Request Accepted" };
            request.PrepeareDeviceRequestAcceptBody(device);
            await _mailService.SendEmailAsync(request);

            UserDevice userDevice = new UserDevice { Device = device, FromUser = ownedBy, ToUser = requester, TransactionDate = DateTime.Now };
            _context.UserDevices.Add(userDevice);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "User,OperationManager")]
        // GET: Devices/Accept/5?userId=12
        public async Task<IActionResult> Deny(int? id, string userId)
        {
            if ((id == null) || (userId == null))
            {
                return NotFound();
            }

            var device = await _context.Devices.FirstAsync(m => m.DeviceId == id);
            var ownedBy = await _context.Users.FirstAsync(ob => ob.Id == device.ApplicationUserId);
            var requester = await _context.Users.FirstAsync(r => r.Id == userId);


            if ((device == null) || (ownedBy == null) || (requester == null))
            {
                return NotFound();
            }

            MailRequest request = new MailRequest { To = "naels141@gmail.com", Subject = "Deivce Request Denied" };
            request.PrepeareDeviceRequestDenyBody(device, ownedBy);
            await _mailService.SendEmailAsync(request);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "User,OperationManager")]
        // GET: Devices/request/5?userId=12
        public async Task<IActionResult> Release(int? id)
        {
            if ((id == null))
            {
                return NotFound();
            }

            var device = await _context.Devices.FirstAsync(m => m.DeviceId == id);
            var ownedBy = await _context.Users.FirstAsync(ob => ob.Id == device.ApplicationUserId);

            if (ownedBy.UserName == "OperationManager")
            {
                return RedirectToAction(nameof(Index));
            }

            var role = _context.Roles.FirstOrDefaultAsync(r => r.Name == "OperationManager");
            var roleUser = _context.UserRoles.FirstOrDefaultAsync(ur => ur.RoleId == role.Result.Id);
            var user = _context.Users.FirstOrDefaultAsync(u => u.Id == roleUser.Result.UserId);

            device.OwnedBy = user.Result;
            device.ApplicationUserId = user.Result.Id;
            device.IsActive = false;
            await _context.SaveChangesAsync();

            if ((device == null) || (ownedBy == null))
            {
                return NotFound();
            }

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
