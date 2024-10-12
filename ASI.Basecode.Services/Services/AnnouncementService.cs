using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AnnouncementService(IAnnouncementRepository announcementRepository, IHttpContextAccessor httpContextAccessor)
        {
            _announcementRepository = announcementRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<AnnouncementViewModel> GetListOfAnnouncements()
        {
            return _announcementRepository.GetAnnouncements()
                .Select(a => new AnnouncementViewModel
                {
                    AnnouncementId = a.AnnouncementId,
                    Title = a.Title,
                    Content = a.Content,
                    CreatedBy = a.Creator.Name,
                    CreatedTime = a.CreatedTime,
                    UpdatedBy = a.UpdatedBy,
                    UpdatedTime = a.UpdatedTime
                }).ToList();
        }

        public void AddAnnouncement(AnnouncementViewModel model)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var announcement = new Announcement
            {
                Title = model.Title,
                Content = model.Content,
                CreatedBy = userId,
                CreatedTime = DateTime.UtcNow
            };
            _announcementRepository.AddAnnouncement(announcement);
        }

        public void EditAnnouncement(AnnouncementViewModel model)
        {
            var announcement = _announcementRepository.GetAnnouncementById(model.AnnouncementId);
            if (announcement == null)
            {
                throw new KeyNotFoundException($"Announcement with ID {model.AnnouncementId} does not exist.");
            }

            announcement.Title = model.Title;
            announcement.Content = model.Content;
            announcement.UpdatedBy = model.UpdatedBy;
            announcement.UpdatedTime = DateTime.UtcNow;

            _announcementRepository.SaveAnnouncement();
        }

        public void DeleteAnnouncement(int announcementId)
        {
            var announcement = _announcementRepository.GetAnnouncementById(announcementId);
            if (announcement == null)
            {
                throw new KeyNotFoundException($"Announcement with ID {announcementId} does not exist.");
            }

            _announcementRepository.DeleteAnnouncement(announcement);
        }
    }
}