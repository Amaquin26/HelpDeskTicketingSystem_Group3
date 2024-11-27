using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class NotificationService: INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public NotificationService(INotificationRepository notificationRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _notificationRepository = notificationRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public List<Notification> GetUserNotifications()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user_notifs = _notificationRepository.GetNotifications().Where(n => n.UserId == userId).ToList();

            return user_notifs;
        }

        public void AddNotification(Notification notification)
        {
            _notificationRepository.AddNotification(notification);
        }

        public void NotifyAgents(string title,string description)
        {
            var agents = _userRepository.GetAgents().ToList();

            var notifs = new List<Notification>();

            foreach (var agent in agents)
            {
                var notif = new Notification { 
                    DateCreated = DateTime.Now, 
                    Title = title, 
                    Description = description, 
                    UserId = agent.UserId,
                };

                notifs.Add(notif);
            }

            _notificationRepository.NotifyAgents(notifs);
        }

        public void DeleteNotification(int notificationId)
        {
            var notification = _notificationRepository.GetNotifications().Where(n => n.Id == notificationId).FirstOrDefault();

            if (notification != null)
            {
                _notificationRepository.DeleteNotification(notification);
            }
        }
    }
}
