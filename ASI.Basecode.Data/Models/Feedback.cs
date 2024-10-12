using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }               
        public int TicketId { get; set; }                  
        public string UserId { get; set; }                    
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedTime { get; set; }


        // Navigation properties
        public Ticket Ticket { get; set; }              
        public User User { get; set; }
    }
}
