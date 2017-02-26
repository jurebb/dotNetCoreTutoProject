using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWorld.ViewModels;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;
using TheWorld.Models;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        private WorldContext _context;

        public AppController(IMailService mailService, IConfigurationRoot config, WorldContext context)     //config da dobavimo adresu za Contact() akvciju iz config jsona ili env var 8Startup.cs)
        {
            _mailService = mailService;
            _config = config;
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.Trips.ToList();
            return View(data);
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if(model.Email.Contains("aol.com"))
            {
                ModelState.AddModelError("Email", "We don't support AOL");
            }
            if (ModelState.IsValid)
            {
                _mailService.SendMail(_config["MailSettings:ToMail"], model.Email, model.Name, model.Message);

                ModelState.Clear();
                ViewBag.UserMessage = "Message sent succesfully!";
            }

            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
