using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Dto
{
    public class UserDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? TeamName { get; set; }
        public string RoleName{ get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }  
    }
}
