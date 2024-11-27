using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Mvc;

namespace ASI.Basecode.WebApp.Controllers
{
    public class UserNotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public UserNotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public IActionResult Index()
        {
            var notifs = _notificationService.GetUserNotifications();
            return View(notifs);
        }

        [HttpPost]
        public IActionResult Delete(int notificationId) 
        {
            _notificationService.DeleteNotification(notificationId);

            return RedirectToAction("Index");
        }
    }
}
