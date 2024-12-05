using ASI.Basecode.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class TicketListViewModel
    {
        public List<TicketDto> Tickets { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchQuery { get; set; }

        // Dynamic data for charts
        public List<TicketCategoryCount> TicketsByCategory { get; set; }
        public List<TicketStatusCount> TicketsByStatus { get; set; }
        public List<TicketPriorityCount> TicketsByPriority { get; set; }
    }

    public class TicketCategoryCount
    {
        public string Category { get; set; }
        public int Count { get; set; }
    }

    public class TicketStatusCount
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }

    public class TicketPriorityCount
    {
        public string Priority { get; set; }
        public int Count { get; set; }
    }

}