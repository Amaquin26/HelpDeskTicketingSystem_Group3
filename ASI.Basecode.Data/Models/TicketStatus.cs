using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public class TicketStatus
    {
        [Key]
        public int StatusId { get; set; }
        public string StatusName { get; set; }

        // Navigation properties
        public ICollection<Ticket> Tickets { get; set; }
    }
}
