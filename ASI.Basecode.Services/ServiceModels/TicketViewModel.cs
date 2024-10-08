using ASI.Basecode.Services.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class TicketViewModel
    {
        public int TicketId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public int StatusId { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }

        [EitherOrRequired(nameof(AssigneeId), nameof(TeamAssignedId),
                      NeitherMessage = "Please provide either an Assignee ID or a Team Assigned ID.",
                      BothMessage = "You cannot assign both an Assignee ID and a Team Assigned ID.")]
        public string? AssigneeId { get; set; }

        [EitherOrRequired(nameof(AssigneeId), nameof(TeamAssignedId),
                      NeitherMessage = "Please provide either an Assignee ID or a Team Assigned ID.",
                      BothMessage = "You cannot assign both an Assignee ID and a Team Assigned ID.")]
        public int? TeamAssignedId { get; set; }
        public string CreatedBy { get; set; }
        public string StatusName { get; set; }
        public string CategoryName { get; set; }
        public string PriorityName { get; set; }
    }
}
