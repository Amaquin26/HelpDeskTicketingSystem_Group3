using ASI.Basecode.Services.Validations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class UserTicketFormModel
    {
        public int TicketId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        public int StatusId { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }

        public int PriorityId { get; set; }
        public string? AssigneeId { get; set; }
        public int? TeamAssignedId { get; set; }

        [MaxFileSize(5)]
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}
