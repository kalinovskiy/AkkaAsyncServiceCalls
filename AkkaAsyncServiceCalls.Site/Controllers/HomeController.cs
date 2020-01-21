using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AkkaAsyncServiceCalls.Site.Models;
using AkkaAsyncServiceCalls.Site.Services;

namespace AkkaAsyncServiceCalls.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISettingsService _settingsService;
        private readonly INotificationService _notificationService;

        public HomeController(
            ISettingsService settingsService,
            INotificationService notificationService)
        {
            _settingsService = settingsService;
            _notificationService = notificationService;
        }

        public IActionResult Index()
        {
            return View(new IndexViewModel
            {
                Persons = _settingsService.GetPersons()
            });
        }

        [HttpPost]
        public IActionResult SendMessage(string messages)
        {
            if (string.IsNullOrWhiteSpace(messages))
            {
                return Ok();
            }

            var persons = _settingsService.GetPersons();

            foreach (var person in persons)
            {
                var msgs = messages.Split(';');
                foreach (var msg in msgs)
                {
                    _notificationService.SendNotification(person, msg);
                }
            }

            return Ok();
        }

        [HttpPost]
        public IActionResult StopMessages()
        {
            _notificationService.Stop();

            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
