using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public class Announcement
    {
        [Key]
        public int AnnouncementId { get; set; }          
        public string Title { get; set; }                
        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }

        // Navigation properties
        public User Creator { get; set; }
    }
}
