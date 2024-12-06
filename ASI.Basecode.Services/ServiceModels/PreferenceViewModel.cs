using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class PreferenceViewModel
    {
        public bool ReceiveNotifications { get; set; } = true;
        public bool TicketViewMode { get; set; } = false;
    }
}
