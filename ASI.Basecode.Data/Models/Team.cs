using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; } 
        public string TeamName { get; set; }
        public string TeamLeaderId { get; set; }
        public string TeamSpecialization { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }

        // Navigation properties
        public User TeamLeader { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
