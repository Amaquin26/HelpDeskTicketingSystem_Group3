using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace ASI.Basecode.WebApp.Controllers
{
    public class UserTicketController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserTicketController(ITicketService ticketService, IHttpContextAccessor httpContextAccessor, ITeamService teamService, IUserService userService, INotificationService notificationService)
        {
            _ticketService = ticketService;
            _teamService = teamService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _notificationService = notificationService;
        }

        public IActionResult Index(string searchQuery, int page = 1, int pageSize = 5)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var tickets = _ticketService.GetListOfTickets().Where(t => t.CreatedById == userId).ToList();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                tickets = tickets.Where(t => t.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                t.CreatedBy.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                t.CategoryName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                t.StatusName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                t.PriorityName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var pagedTickets = tickets.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var totalPages = (int)Math.Ceiling(tickets.Count / (double)pageSize);

            var viewModel = new UserTicketListViewModel
            {
                Tickets = pagedTickets,
                CurrentPage = page,
                TotalPages = totalPages,
                SearchQuery = searchQuery,
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var ticketModel = new UserTicketViewModel();

            var ticketStatuses = _ticketService.GetTicketStatusList();
            var ticketPriorities = _ticketService.GetTicketPriorityList();
            var ticketCategories = _ticketService.GetTicketCategoryList();

            ticketModel.Ticket = new UserTicketFormModel();
            ticketModel.TicketPriorities = ticketPriorities;
            ticketModel.TicketCategories = ticketCategories;
            ticketModel.TicketStatuses = ticketStatuses;
            ticketModel.Agents = _userService.GetAgents().Where(x => x.RoleId == 3).ToList();
            ticketModel.Teams = _teamService.GetListOfTeams().Select(t => new Team
            {
                TeamId = t.TeamId,
                TeamName = t.TeamName,
            }).ToList();

            return View(ticketModel);

        }

        [HttpPost]
        public IActionResult Create(UserTicketViewModel ticketModel)
        {
            // Set to open by default
            ticketModel.Ticket.StatusId = 1;
            ticketModel.Ticket.PriorityId = 1;

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

                if (ticketModel.Agents.Count == 0)
                {
                    ticketModel.Agents = _userService.GetAgents().ToList();
                }

                if (ticketModel.Teams.Count == 0)
                {
                    ticketModel.Teams = _teamService.GetListOfTeams().Select(t => new Team
                    {
                        TeamId = t.TeamId,
                        TeamName = t.TeamName,
                    }).ToList();
                }

                return View(ticketModel);
            }

            var ticketFormModel = ticketModel.Ticket;

            var ticket = new TicketFormModel
            {
                Title = ticketFormModel.Title,
                Description = ticketFormModel.Description,
                PriorityId = 1,
                CategoryId = ticketFormModel.CategoryId,
                StatusId = 1,
                AssigneeId = ticketFormModel.AssigneeId,
                Files = ticketFormModel.Files,
            };

            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userService.GetUsers().FirstOrDefault(x => x.UserId == userId);

            _ticketService.AddTicket(ticket);
            _notificationService.NotifyAgents("New Ticket Created", $"A new ticket has been created by {user?.Name}");
            return RedirectToAction("Index");
        }
    }
}
