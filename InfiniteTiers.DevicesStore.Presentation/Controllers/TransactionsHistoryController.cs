using InfiniteTiers.DevicesStore.Data.Models;
using InfiniteTiers.DevicesStore.Logic.Repositories;
using InfiniteTiers.DevicesStore.Presentation.Models;
using InfiniteTiers.DevicesStore.Presentation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InfiniteTiers.DevicesStore.Presentation.Controllers
{
    [Authorize]
    public class TransactionsHistoryController : Controller
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDeviceHistoryRepository _historyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;

        public TransactionsHistoryController(IDeviceRepository deviceRepository,
                                            IDeviceHistoryRepository historyRepository,
                                            IUserRepository userRepository,
                                            IMailService mailService)
        {
            _deviceRepository = deviceRepository;
            _historyRepository = historyRepository;
            _userRepository = userRepository;
            _mailService = mailService;
        }

        [Authorize(Roles = "Admin")]
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
            var device = _deviceRepository.GetById(id);
            ViewData["Device"] = device.Name;

            return View(history);
        }

        [Authorize(Roles = "User,OperationManager")]
        // GET: Devices/request/5?userId=12
        public async Task<IActionResult> RequestDevice(int? id, string userId)
        {
            if ((id == null) || (userId == null))
            {
                return NotFound();
            }

            var device = _deviceRepository.GetById(id);
            var requester = _userRepository.GetUserById(userId);
            if ((device == null) || (requester == null))
            {
                return NotFound();
            }

            MailRequest request = new MailRequest { To = "naels141@gmail.com", Subject = "Deivce Request" };
            request.PrepeareDeviceRequestBody(device, requester);
            await _mailService.SendEmailAsync(request);


            return RedirectToAction(nameof(Index), "Devices");
        }

        [Authorize(Roles = "User,OperationManager")]
        // GET: Devices/Accept/5?userId=12
        public async Task<IActionResult> Accept(int? id, string userId)
        {
            if ((id == null) || (userId == null))
            {
                return NotFound();
            }

            var device = _deviceRepository.GetById(id);
            var requester = _userRepository.GetUserById(userId);

            if ((device == null)|| (requester == null))
            {
                return NotFound();
            }

            if (device.OwnedBy.Equals(requester))
            {
                return BadRequest();
            }

            UserDevice userDevice = new UserDevice { Device = device, FromUser = device.OwnedBy, ToUser = requester, TransactionDate = DateTime.Now };
            if (!_historyRepository.Save(userDevice))
            {
                return BadRequest();
            }

            device.OwnedBy = requester;
            if (device.OwnedBy.UserName == "OperationManager")
            {
                device.IsActive = false;
            }
            else
            {
                device.IsActive = true;
            }

            if (!_deviceRepository.Update(device))
            {
                return BadRequest();
            }



            MailRequest request = new MailRequest { To = "naels141@gmail.com", Subject = "Deivce Request Accepted" };
            request.PrepeareDeviceRequestAcceptBody(device);
            await _mailService.SendEmailAsync(request);

            return RedirectToAction(nameof(Index), "Devices");
        }

        [Authorize(Roles = "User,OperationManager")]
        // GET: Devices/Accept/5?userId=12
        public async Task<IActionResult> Deny(int? id, string userId)
        {
            if ((id == null) || (userId == null))
            {
                return NotFound();
            }

            var device = _deviceRepository.GetById(id);
            var requester = _userRepository.GetUserById(userId);


            if ((device == null) || (requester == null))
            {
                return NotFound();
            }

            MailRequest request = new MailRequest { To = "naels141@gmail.com", Subject = "Deivce Request Denied" };
            request.PrepeareDeviceRequestDenyBody(device);
            await _mailService.SendEmailAsync(request);

            return RedirectToAction(nameof(Index), "Devices");
        }

        [Authorize(Roles = "User")]
        // GET: Devices/request/5?userId=12
        public IActionResult ReleaseDevice(int? id)
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

            var role = _userRepository.GetRoleByUser(device.OwnedBy.Id);
            if (role.Name == "OperationManager")
            {
                return RedirectToAction(nameof(Index), "Devices");
            }

            var user = _userRepository.GetUserByRole("OperationManager");
            if (user == null)
            {
                return BadRequest();
            }

            UserDevice userDevice = new UserDevice { Device = device, FromUser = device.OwnedBy, ToUser = user, TransactionDate = DateTime.Now };
            if (!_historyRepository.Save(userDevice))
            {
                return BadRequest();
            }

            device.OwnedBy = user;
            device.IsActive = false;

            if (!_deviceRepository.Update(device))
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index), "Devices");
        }

    }
}
