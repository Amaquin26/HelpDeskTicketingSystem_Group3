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
        public List<TicketPriority> TicketPriorities { get; set; } = new List<TicketPriority>();

        // Temporary values please change when AgentList and TeamList is available
        public List<User> Agents { get; set; } = new List<User> { 
            new User
            {
                UserId = "6a7ee692-8894-4e93-9c5f-c22dc89f2d44",
                Name = "Agent"
            }
        };
        public List<Team> Teams { get; set; } = new List<Team> { 
            new Team
            {
                TeamId = 1,
                TeamName = "Team A"
            }
        };
    }
}
