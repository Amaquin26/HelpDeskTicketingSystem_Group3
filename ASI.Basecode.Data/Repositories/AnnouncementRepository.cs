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
    public class AnnouncementRepository : BaseRepository, IAnnouncementRepository
    {
        public AnnouncementRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
         
        public IQueryable<Announcement> GetAnnouncements()
        {
            return GetDbSet<Announcement>().Include(a => a.Creator);
        }

        public bool AnnouncementExists(int announcementId)
        {
            return GetDbSet<Announcement>().Any(a => a.AnnouncementId == announcementId);
        }

        public Announcement? GetAnnouncementById(int announcementId)
        {
            return GetDbSet<Announcement>().Include(a => a.Creator)
                .FirstOrDefault(a => a.AnnouncementId == announcementId);
        }

        public void AddAnnouncement(Announcement announcement)
        {
            GetDbSet<Announcement>().Add(announcement);
            SaveAnnouncement();
        }

        public void DeleteAnnouncement(Announcement announcement)
        {
            GetDbSet<Announcement>().Remove(announcement);
            SaveAnnouncement();
        }

        public void SaveAnnouncement()
        {
            UnitOfWork.SaveChanges();
        }
    }
}
