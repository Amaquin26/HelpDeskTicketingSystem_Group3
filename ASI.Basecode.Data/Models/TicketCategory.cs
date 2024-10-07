using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public class TicketCategory
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        // Navigation properties
        public ICollection<Ticket> Tickets { get; set; }
    }
}
