using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IAnnouncementRepository
    {
        IQueryable<Announcement> GetAnnouncements();
        bool AnnouncementExists(int announcementId);
        void AddAnnouncement(Announcement announcement);
        Announcement? GetAnnouncementById(int announcementId);
        void DeleteAnnouncement(Announcement announcement);
        void SaveAnnouncement();
    }
}
