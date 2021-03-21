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
using InfiniteTiers.DevicesStore.Logic.Repositories;
using System.Collections.Generic;

namespace InfiniteTiers.DevicesStore.Presentation.Controllers
{
    [Authorize]
    public class DevicesController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IDeviceRepository _deviceRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDeviceHistoryRepository _historyRepository;

        public DevicesController(IMailService mailService, IDeviceRepository deviceRepository,
                                 ICategoryRepository categoryRepository, IUserRepository userRepository,
                                 IDeviceHistoryRepository historyRepository)
        {
            _mailService = mailService;
            _deviceRepository = deviceRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _historyRepository = historyRepository;

        }

        // GET: Devices
        public IActionResult Index(string searchString)
        {
            var devices = _deviceRepository.GetDevices();
            if (!String.IsNullOrEmpty(searchString))
            {
                devices = devices.Where(d => d.Name.Contains(searchString));
            }
            return View(devices.ToList());
        }

        [Authorize(Roles = "Admin,OperationManager")]
        // GET: Devices/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = _deviceRepository.GetDevice(id);
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
        public IActionResult Create([Bind("Name,Description,Manufacturer,Model,SerialNumber,PurchaseDate,CategoryId")] Device device)
        {

            if (ModelState.IsValid)
            {
                var user = _userRepository.GetUserByRole("OperationManager");
                device.ApplicationUserId = user.Id;
                device.IsActive = false;

                _deviceRepository.SaveDevice(device);
                return RedirectToAction(nameof(Index));
            }
            PopulateCategoriesDropDownList(device.CategoryId);
            return View(device);
        }

        [Authorize(Roles = "Admin")]

        // GET: Devices/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = _deviceRepository.GetDevice(id);

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
        public IActionResult EditPost(int id, [Bind("DeviceId,Name,Description,Manufacturer,Model,SerialNumber,PurchaseDate,IsActive,CategoryId,ApplicationUserId")] Device device)
        {
            if (id != device.DeviceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _deviceRepository.UpdateDevice(device);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(device.DeviceId))
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

            PopulateCategoriesDropDownList(device.CategoryId);
            return View(device);
        }

        [Authorize(Roles = "Admin")]

        // GET: Devices/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = _deviceRepository.GetDevice(id);
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
        public IActionResult DeleteConfirmed(int id)
        {
            _deviceRepository.DeleteDevice(id);
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

            var device = _deviceRepository.GetDevice(id);
            var ownedBy = _userRepository.GetUserById(device.ApplicationUserId);
            var requester = _userRepository.GetUserById(userId);

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

            var device = _deviceRepository.GetDevice(id);
            var ownedBy = _userRepository.GetUserById(device.ApplicationUserId);
            var requester = _userRepository.GetUserById(userId);



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

            _deviceRepository.UpdateDevice(device);

            MailRequest request = new MailRequest { To = "naels141@gmail.com", Subject = "Deivce Request Accepted" };
            request.PrepeareDeviceRequestAcceptBody(device);
            await _mailService.SendEmailAsync(request);

            UserDevice userDevice = new UserDevice { Device = device, FromUser = ownedBy, ToUser = requester, TransactionDate = DateTime.Now };
            _historyRepository.SaveDeviceHistory(userDevice);

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

            var device = _deviceRepository.GetDevice(id);
            var ownedBy = _userRepository.GetUserById(device.ApplicationUserId);
            var requester = _userRepository.GetUserById(userId);


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
        public IActionResult Release(int? id)
        {
            if ((id == null))
            {
                return NotFound();
            }

            var device = _deviceRepository.GetDevice(id);
            var ownedBy = _userRepository.GetUserById(device.ApplicationUserId);

            var role = _userRepository.GetRoleByUser(ownedBy.Id);
            if (role.Name == "OperationManager")
            {
                return RedirectToAction(nameof(Index));
            }

            var user = _userRepository.GetUserByRole("OperationManager");

            device.OwnedBy = user;
            device.ApplicationUserId = user.Id;
            device.IsActive = false;

            _deviceRepository.UpdateDevice(device);

            UserDevice userDevice = new UserDevice { Device = device, FromUser = ownedBy, ToUser = user, TransactionDate = DateTime.Now };
            _historyRepository.SaveDeviceHistory(userDevice);

            return RedirectToAction(nameof(Index));
        }

        private bool DeviceExists(int id)
        {
            return _deviceRepository.DeviceExists(id);
        }

        private void PopulateCategoriesDropDownList(object selectedCategory = null)
        {
            var categoriesQuery = _categoryRepository.GetAll().OrderBy(c => c.Name);
            ViewBag.CategoryId = new SelectList(categoriesQuery, "CategoryId", "Name", selectedCategory);
        }

    }
}
