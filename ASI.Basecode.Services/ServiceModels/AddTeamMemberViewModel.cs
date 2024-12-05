using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class AddTeamMemberViewModel
    {
        [Required(ErrorMessage = "User is required.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Team is required.")]
        public int TeamId { get; set; }
    }
}
