using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class PagedTeamViewModel
    {
        public List<TeamViewModel> teams { get; set; }
        public int totalPages { get; set; }
    }
}
