using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers
{
    public class TicketController : ControllerBase<TicketController>
    {
        private readonly ITicketService _ticketService;

        public TicketController(
            IHttpContextAccessor httpContextAccessor, 
            ILoggerFactory loggerFactory, 
            IConfiguration configuration,
            ITicketService ticketService,
            IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _ticketService = ticketService;
        }

        public IActionResult Index()
        {
            var ticketModel = new TicketViewModel();

            var ticketStatuses = _ticketService.GetTicketStatusList();
            var ticketPriorities = _ticketService.GetTicketPriorityList();
            var ticketCategories = _ticketService.GetTicketCategoryList();

            ticketModel.Ticket = new TicketFormModel();
            ticketModel.ticketPriorities = ticketPriorities;
            ticketModel.TicketCategories = ticketCategories;
            ticketModel.TicketStatuses = ticketStatuses;    

            return View(ticketModel);
        }

        [HttpGet]
        public JsonResult GetTickets()
        {
            var tickets = _ticketService.GetListOfTickets();
            return Json(tickets); 
        }

        [HttpPost]
        public IActionResult AddTicket(TicketViewModel ticketModel)
        {
            var ticketFormModel = ticketModel.Ticket;

            var ticket = new TicketFormModel
            {
                Title = ticketFormModel.Title,
                Description = ticketFormModel.Description,
                PriorityId = ticketFormModel.PriorityId,
                CategoryId = ticketFormModel.CategoryId,
                //StatusId = ticketFormModel.StatusId,
                //AssigneeId = ticketFormModel.AssigneeId,
                StatusId = 1,
                AssigneeId = "bf99147b-422b-4ea6-874b-7ae5836eea95",
                TeamAssignedId = ticketFormModel.TeamAssignedId,
            };

            _ticketService.AddTicket(ticket);
            return RedirectToAction("Index");
        }
    }
}
