using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface INotificationRepository
    {
        IQueryable<Notification> GetNotifications();
        void AddNotification(Notification notification);
        void NotifyAgents(List<Notification> notifications);
        void DeleteNotification(Notification notification);
    }
}
