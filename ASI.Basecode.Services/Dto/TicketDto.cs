using ASI.Basecode.Services.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Dto
{
    public class TicketDto
    {
        public int TicketId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AssigneeId { get; set; }
        public int? TeamAssignedId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public string StatusName { get; set; }
        public string CategoryName { get; set; }
        public string PriorityName { get; set; }
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public int PriorityId { get; set; }
        public string? AgentName { get; set; }
        public string? TeamName { get; set; }
        public DateTime DateAdded { get; set; }
        public bool CanEdit { get; set; } = false;
    }
}
