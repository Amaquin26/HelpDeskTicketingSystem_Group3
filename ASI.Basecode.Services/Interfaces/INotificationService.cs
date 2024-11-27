using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface INotificationService
    {
        List<Notification> GetUserNotifications();
        void AddNotification(Notification notification);
        void NotifyAgents(string title, string description);
        void DeleteNotification(int notificationId);
    }
}
