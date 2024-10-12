using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class TeamViewModel
    {
        public int TeamId { get; set; }

        [Required(ErrorMessage = "Team name is required.")]
        public string TeamName { get; set; }

        public string TeamLeaderId { get; set; }

        [Required(ErrorMessage = "Team specialization is required.")]
        public string TeamSpecialization { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; }
        public string TeamLeaderName { get; set; }
    }

}
