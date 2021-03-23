using InfiniteTiers.DevicesStore.Data.Models;
using InfiniteTiers.DevicesStore.Logic.Repositories;
using InfiniteTiers.DevicesStore.Presentation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

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
            var devices = _deviceRepository.GetAll();
            if (!String.IsNullOrEmpty(searchString))
            {
                devices = devices.Where(d => d.Name.Contains(searchString));
            }
            return View(devices.ToList().Take(10));
        }

        [Authorize(Roles = "Admin,OperationManager")]
        // GET: Devices/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = _deviceRepository.GetById(id);
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
                if (user == null)
                {
                    return NotFound();
                }
                device.OwnedBy = user;
                device.IsActive = false;

                if (_deviceRepository.Save(device))
                {
                    return RedirectToAction(nameof(Index));
                }
                return BadRequest();
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

            var device = _deviceRepository.GetById(id);

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
                if (!_deviceRepository.IsExist(device.DeviceId))
                {
                    return NotFound();
                }
                else if (_deviceRepository.Update(device))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return BadRequest();
                }
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

            var device = _deviceRepository.GetById(id);
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
            if (_deviceRepository.Delete(id))
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest();
            }
        }

        private void PopulateCategoriesDropDownList(object selectedCategory = null)
        {
            var categoriesQuery = _categoryRepository.GetAll().OrderBy(c => c.Name);
            ViewBag.CategoryId = new SelectList(categoriesQuery, "CategoryId", "Name", selectedCategory);
        }

    }
}
