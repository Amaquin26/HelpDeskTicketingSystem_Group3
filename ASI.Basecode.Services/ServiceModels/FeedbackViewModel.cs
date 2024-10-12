using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class FeedbackViewModel
    {
        public int FeedbackId { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }

        [Required(ErrorMessage = "Comment is required.")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        public int Rating { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string TicketTitle { get; set; } 
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
