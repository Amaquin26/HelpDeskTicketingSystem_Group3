using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Dto
{
    public class UserDetailsDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string? TeamName { get; set; }
        public int? TeamId { get; set; }
        public int TicketsResolved { get; set; }
        public TimeSpan AverageResolutionTime { get; set; }
        public string AverageResolutionTimeString { get; set; }
        public float CustomerRating { get; set; }
        public List<TicketDto> Tickets { get; set; } = new List<TicketDto>();

    }
}
