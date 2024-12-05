using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class TeamMember
    {
        public string UserId { get; set; }
        public string Name { get; set; }    
        public string Role { get; set; }
        public string Email { get; set; }
    }
}
