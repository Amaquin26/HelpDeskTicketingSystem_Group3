using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class NotificationRepository : BaseRepository, INotificationRepository
    {
        public NotificationRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IQueryable<Notification> GetNotifications()
        {
            return GetDbSet<Notification>();
        }

        public void AddNotification(Notification notification)
        {
            GetDbSet<Notification>().Add(notification);
            SaveNotification();
        }

        public void NotifyAgents(List<Notification> notifications)
        {
            GetDbSet<Notification>().AddRange(notifications);
            SaveNotification();
        }

        public void DeleteNotification(Notification notification)
        {
            GetDbSet<Notification>().Remove(notification);
            SaveNotification();
        }

        public void SaveNotification()
        {
            UnitOfWork.SaveChanges();
        }
    }
}
