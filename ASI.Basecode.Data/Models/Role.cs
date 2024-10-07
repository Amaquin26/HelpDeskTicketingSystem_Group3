using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }           
        public string RoleName { get; set; }


        // Navigation properties
        public ICollection<User> Users { get; set; }
    }
}
