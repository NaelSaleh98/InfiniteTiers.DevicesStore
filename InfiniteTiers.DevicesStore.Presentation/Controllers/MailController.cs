using InfiniteTiers.DevicesStore.Presentation.Models;
using InfiniteTiers.DevicesStore.Presentation.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniteTiers.DevicesStore.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : Controller
    {
    private readonly IMailService mailService;
    public MailController(IMailService mailService)
    {
        this.mailService = mailService;
    }
    [HttpPost("send")]
    public async Task<IActionResult> SendMail()
    {
        try
        {
                MailRequest request = new MailRequest { To = "naels141@gmail.com", Subject = "Hello World", Body = "Hello there" };
            await mailService.SendEmailAsync(request);
            return Ok();
        }
        catch (Exception ex)
        {
            throw;
        }
            
    }
    }
}
