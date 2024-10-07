using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public string? AssigneeId { get; set; }
        public int? TeamAssignedId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public DateTime? ResolvedTime { get; set; }

        // Navigation properties
        public User Assignee { get; set; }
        public Team Team { get; set; }
        public TicketCategory Category { get; set; }
        public TicketStatus Status { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
    }
}
