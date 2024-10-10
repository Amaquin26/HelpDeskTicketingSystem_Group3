using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASI.Basecode.Data.Models
{
    public partial class User
    {
        [Key]
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int? TeamId { get; set; }
        public int RoleId { get; set; }
        public string? Email { get; set; } 
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<Team> TeamsLed { get; set; } 
        public ICollection<Team> Teams { get; set; }
        public Role Role { get; set; }
    }
}
