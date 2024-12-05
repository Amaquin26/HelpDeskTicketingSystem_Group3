using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Dto
{
    public class TeamDetailsDto
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string? LeaderEmail { get; set; }
        public string? LeaderName { get; set; }
        public string? TeamLeaderId { get; set; }
        public string Specialization { get; set; }
        public int TicketsResolved { get; set; }
        public TimeSpan AverageResolutionTime { get; set; }
        public string AverageResolutionTimeString { get; set; }
        public float CustomerRating { get; set; }
        public List<TicketDto> Tickets { get; set; } = new List<TicketDto>();
        public List<TeamMember> TeamMebers { get; set; } = new List<TeamMember>();
    }
}
