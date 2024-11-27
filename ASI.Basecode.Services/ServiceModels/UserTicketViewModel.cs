using ASI.Basecode.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class UserTicketViewModel
    {
        public UserTicketFormModel Ticket { get; set; }
        public List<TicketCategory> TicketCategories { get; set; } = new List<TicketCategory>();
        public List<TicketStatus> TicketStatuses { get; set; } = new List<TicketStatus>();
        public List<TicketPriority> TicketPriorities { get; set; } = new List<TicketPriority>();
        public List<User> Agents { get; set; } = new List<User>();
        public List<Team> Teams { get; set; } = new List<Team>();
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}
