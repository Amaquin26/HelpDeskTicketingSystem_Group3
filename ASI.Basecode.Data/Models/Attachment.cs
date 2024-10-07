using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public class Attachment
    {
        [Key]
        public int Id { get; set; }     
        public string FileName { get; set; }  
        public string FilePath { get; set; }
        public int TicketId { get; set; }

        // Navigation properties
        public Ticket Ticket { get; set; }
    }
}
