using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class TicketViewModel
    {
        public TicketFormModel Ticket { get; set; }
        public List<TicketCategory> TicketCategories { get; set; } = new List<TicketCategory>();
        public List<TicketStatus> TicketStatuses { get; set; } = new List<TicketStatus>();
        public List<TicketPriority> ticketPriorities { get; set; } = new List<TicketPriority>();
    }
}
