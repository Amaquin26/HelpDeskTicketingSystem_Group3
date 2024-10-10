using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Models;
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

        [HttpGet]
        public IActionResult Index()
        {
            var tickets = _ticketService.GetListOfTickets();

            return View(tickets);
        }

        [HttpGet]
        public IActionResult AddTicket()
        {
            var ticketModel = new TicketViewModel();

            var ticketStatuses = _ticketService.GetTicketStatusList();
            var ticketPriorities = _ticketService.GetTicketPriorityList();
            var ticketCategories = _ticketService.GetTicketCategoryList();

            ticketModel.Ticket = new TicketFormModel();
            ticketModel.TicketPriorities = ticketPriorities;
            ticketModel.TicketCategories = ticketCategories;
            ticketModel.TicketStatuses = ticketStatuses;

            return View(ticketModel);
        }

        [HttpGet]
        public IActionResult EditTicket(int ticketId)
        {
            var ticketModel = new TicketViewModel();

            var ticketStatuses = _ticketService.GetTicketStatusList();
            var ticketPriorities = _ticketService.GetTicketPriorityList();
            var ticketCategories = _ticketService.GetTicketCategoryList();
            var result = _ticketService.GetTicketById(ticketId);

            var ticket = result.Item1;

            if (ticket == null)
            {
                return NotFound(new { Message = "Ticket not found" });
            }

            ticketModel.Ticket = new TicketFormModel
            {
                TicketId = ticket.TicketId,
                Title = ticket.Title,
                Description = ticket.Description,
                AssigneeId = ticket.AssigneeId,
                TeamAssignedId = ticket.TeamAssignedId,
                CategoryId = ticket.CategoryId,
                StatusId = ticket.StatusId,
                PriorityId = ticket.StatusId
            };
            ticketModel.TicketPriorities = ticketPriorities;
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
            // Set to open by default
            ticketModel.Ticket.StatusId = 1;

            //Return the view with the model to show validation errors
            if (!ModelState.IsValid)
            {
                if (ticketModel.TicketStatuses.Count == 0)
                {
                    ticketModel.TicketStatuses = _ticketService.GetTicketStatusList();
                }

                if (ticketModel.TicketPriorities.Count == 0)
                {
                    ticketModel.TicketPriorities = _ticketService.GetTicketPriorityList();
                }

                if (ticketModel.TicketCategories.Count == 0)
                {
                    ticketModel.TicketCategories = _ticketService.GetTicketCategoryList();
                }

                return View(ticketModel);
            }

            var ticketFormModel = ticketModel.Ticket;

            var ticket = new TicketFormModel
            {
                Title = ticketFormModel.Title,
                Description = ticketFormModel.Description,
                PriorityId = ticketFormModel.PriorityId,
                CategoryId = ticketFormModel.CategoryId,
                StatusId = 1,
                AssigneeId = ticketFormModel.AssigneeId,
                TeamAssignedId = ticketFormModel.TeamAssignedId,
            };

            _ticketService.AddTicket(ticket);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditTicket(TicketViewModel ticketModel)
        {
            //Return the view with the model to show validation errors
            if (!ModelState.IsValid)
            {
                if (ticketModel.TicketStatuses.Count == 0)
                {
                    ticketModel.TicketStatuses = _ticketService.GetTicketStatusList();
                }

                if (ticketModel.TicketPriorities.Count == 0)
                {
                    ticketModel.TicketPriorities = _ticketService.GetTicketPriorityList();
                }

                if (ticketModel.TicketCategories.Count == 0)
                {
                    ticketModel.TicketCategories = _ticketService.GetTicketCategoryList();
                }

                return View(ticketModel);
            }

            var ticket = new TicketFormModel
            {
                TicketId = ticketModel.Ticket.TicketId,
                Title = ticketModel.Ticket.Title,
                Description = ticketModel.Ticket.Description,
                AssigneeId = ticketModel.Ticket.AssigneeId,
                TeamAssignedId = ticketModel.Ticket.TeamAssignedId,
                CategoryId = ticketModel.Ticket.CategoryId,
                StatusId = ticketModel.Ticket.StatusId,
                PriorityId = ticketModel.Ticket.StatusId
            };

            _ticketService.EditTicket(ticket);
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public IActionResult DeleteTicket([FromBody] TicketDeleteRequest request)
        {
            _ticketService.DeleteTicket(request.TicketId);
            return Ok();
        }
    }
}
