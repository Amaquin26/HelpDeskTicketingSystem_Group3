using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Models;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System;

namespace ASI.Basecode.WebApp.Controllers
{
    public class TicketController : ControllerBase<TicketController>
    {
        private readonly ITicketService _ticketService;
        private readonly IUserService _userService;
        private readonly ITeamService _teamService;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _contextAccessor;

        public TicketController(
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            ITicketService ticketService,
            IUserService userService,
            ITeamService teamService,
            INotificationService notificationService,
            IHttpContextAccessor contextAccessor,
        IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _ticketService = ticketService;
            _userService = userService;
            _teamService = teamService;
            _notificationService = notificationService;
            _contextAccessor = contextAccessor;
        }

        [HttpGet]
        public IActionResult Index(string searchQuery, int page = 1, int pageSize = 5)
        {                
            var tickets = _ticketService.GetListOfTickets();

            // Generate chart data for Tickets by Category, Status, and Priority
            var ticketsByCategory = tickets
                .GroupBy(t => t.CategoryName)
                .Select(g => new TicketCategoryCount { Category = g.Key, Count = g.Count() })
                .ToList();

            var ticketsByStatus = tickets
                .GroupBy(t => t.StatusName)
                .Select(g => new TicketStatusCount { Status = g.Key, Count = g.Count() })
                .ToList();

            var ticketsByPriority = tickets
                .GroupBy(t => t.PriorityName)
                .Select(g => new TicketPriorityCount { Priority = g.Key, Count = g.Count() })
                .ToList();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                tickets = tickets.Where(t => t.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                t.CreatedBy.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                t.AgentName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                t.CategoryName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                t.StatusName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                t.PriorityName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Paginate the tickets
            var pagedTickets = tickets.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var totalPages = (int)Math.Ceiling(tickets.Count / (double)pageSize);

            var viewModel = new TicketListViewModel
            {
                Tickets = pagedTickets,
                CurrentPage = page,
                TotalPages = totalPages,
                SearchQuery = searchQuery,
                TicketsByCategory = ticketsByCategory,
                TicketsByStatus = ticketsByStatus,
                TicketsByPriority = ticketsByPriority
            };

            return View(viewModel);
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
            ticketModel.Agents = _userService.GetAgents().Where(x => x.RoleId == 3).ToList();
            ticketModel.Teams = _teamService.GetListOfTeams().Select(t => new Team
            {
                TeamId = t.TeamId,
                TeamName = t.TeamName,
            }).ToList();

            return View(ticketModel);
        }

        [HttpGet]
        public IActionResult EditTicket(int id)
        {
            var ticketModel = new TicketViewModel();

            var ticketStatuses = _ticketService.GetTicketStatusList();
            var ticketPriorities = _ticketService.GetTicketPriorityList();
            var ticketCategories = _ticketService.GetTicketCategoryList();
            var result = _ticketService.GetTicketById(id);

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
            ticketModel.Agents = _userService.GetAgents().Where(x => x.RoleId == 3).ToList();
            ticketModel.Teams = _teamService.GetListOfTeams().Select(t => new Team
            {
                TeamId = t.TeamId,
                TeamName = t.TeamName,
            }).ToList();

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
                PriorityId = ticketFormModel.PriorityId,
                CategoryId = ticketFormModel.CategoryId,
                StatusId = 1,
                AssigneeId = ticketFormModel.AssigneeId,
                Files = ticketModel.Files,
            };

            _ticketService.AddTicket(ticket);
            _notificationService.AddNotification(
                new Notification
                {
                    Title = "You have been assigned to a ticket",
                    Description = "New ticket was assigned to you",
                    DateCreated = System.DateTime.Now,
                    UserId=ticket.AssigneeId
                }
            );
            TempData["SuccessMessage"] = "Ticket Added";
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
            _notificationService.AddNotification(
                new Notification
                {
                    Title = "Ticket updated",
                    Description = "Ticket that was assigned to you was updated",
                    DateCreated = System.DateTime.Now,
                    UserId = ticket.AssigneeId
                }
            );

            TempData["SuccessMessage"] = "Ticket Updated";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteTicket(int ticketId)
        {
            // Find the ticket by ID
            var result = _ticketService.GetTicketById(ticketId);

            if (result.Item2 == false)
            {
                return NotFound();
            }

            // Delete the ticket
            _ticketService.DeleteTicket(ticketId);

            TempData["SuccessMessage"] = "Ticket Deleted";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ViewTicket(int id)
        {
            var ticketModel = _ticketService.GetTicketById(id);

            var ticket = ticketModel.Item1;

            return View(ticket);
        }
    }
}
