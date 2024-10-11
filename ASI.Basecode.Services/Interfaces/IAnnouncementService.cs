using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IAnnouncementService
    {
        List<AnnouncementViewModel> GetListOfAnnouncements();
        void AddAnnouncement(AnnouncementViewModel model);
        void EditAnnouncement(AnnouncementViewModel model);
        void DeleteAnnouncement(int announcementId);
    }
}
