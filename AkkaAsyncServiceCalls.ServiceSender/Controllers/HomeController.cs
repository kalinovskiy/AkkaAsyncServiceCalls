using System.Diagnostics;
using AkkaAsyncServiceCalls.ServiceSender.Models;
using AkkaAsyncServiceCalls.ServiceSender.Services;
using Microsoft.AspNetCore.Mvc;

namespace AkkaAsyncServiceCalls.ServiceSender.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISettings _settings;

        public HomeController(ISettings settings)
        {
            _settings = settings;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new IndexViewModel
            {
                IsEmailActive = _settings.IsEmailSenderActive,
                IsSmsActive = _settings.IsSmsSenderActive
            });
        }

        [HttpPost]
        public IActionResult ToggleEmailState()
        {
            _settings.IsEmailSenderActive = !_settings.IsEmailSenderActive;

            return Ok(_settings.IsEmailSenderActive);
        }

        [HttpPost]
        public IActionResult ToggleSmsState()
        {
            _settings.IsSmsSenderActive = !_settings.IsSmsSenderActive;

            return Ok(_settings.IsSmsSenderActive);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
