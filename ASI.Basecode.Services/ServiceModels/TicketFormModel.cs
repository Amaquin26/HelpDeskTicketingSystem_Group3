﻿using ASI.Basecode.Data.Models;
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
    [EitherOrRequired(nameof(AssigneeId), nameof(TeamAssignedId),
              NeitherMessage = "Please provide either an Assignee ID or a Team Assigned ID.",
              BothMessage = "You cannot assign both an Assignee ID and a Team Assigned ID.")]
    public class TicketFormModel
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

        [Required(ErrorMessage = "Priority is required.")]
        public int PriorityId { get; set; }
        public string? AssigneeId { get; set; }
        public int? TeamAssignedId { get; set; }

        [MaxFileSize(5)]
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}
