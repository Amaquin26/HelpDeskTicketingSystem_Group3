using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Claims;
namespace ASI.Basecode.Services.Services
{
    public class TicketService:ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TicketService(ITicketRepository ticketRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<TicketViewModel> GetListOfTickets()
        {
            var tickets = _ticketRepository.GetTickets().Join(_userRepository.GetUsers(),
                  ticket => ticket.CreatedBy, 
                  user => user.UserId,    
                  (ticket, user) => new TicketViewModel
                  {
                      TicketId = ticket.TicketId,
                      Title = ticket.Title,
                      Description = ticket.Description,
                      TeamAssignedId = ticket.TeamAssignedId,
                      AssigneeId = ticket.AssigneeId,
                      CategoryId = ticket.CategoryId,
                      StatusId = ticket.StatusId,
                      CreatedBy = user.Name,
                      StatusName = ticket.Status.StatusName,
                      CategoryName = ticket.Category.CategoryName,
                      PriorityName = ticket.Priority.PriorityName
                  })
            .ToList();

            return tickets;
        }

        public void AddTicket(TicketViewModel model)
        {
            var ticket = new Ticket();

            ticket.Title = model.Title;
            ticket.Description = model.Description;
            ticket.AssigneeId = model.AssigneeId;
            ticket.TeamAssignedId = model.TeamAssignedId;
            ticket.StatusId = model.StatusId;
            ticket.CategoryId = model.CategoryId;
            ticket.CreatedTime = DateTime.UtcNow;

            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ticket.CreatedBy = userId;

            _ticketRepository.AddTicket(ticket);
        }

        public void EditTicket(TicketViewModel model)
        {

            var ticket = _ticketRepository.GetTicketById(model.TicketId);

            if(ticket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {model.TicketId} does not exist.");
            }

            ticket.Title = model.Title;
            ticket.Description = model.Description;
            ticket.AssigneeId = model.AssigneeId;
            ticket.TeamAssignedId = model.TeamAssignedId;
            ticket.StatusId = model.StatusId;
            ticket.CategoryId = model.CategoryId;
            ticket.UpdatedTime = DateTime.UtcNow;

            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ticket.UpdatedBy = userId;

            _ticketRepository.SaveTicket();
        }

        public void DeleteTicket(int ticketId) 
        {
            var ticket = _ticketRepository.GetTicketById(ticketId);

            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {ticketId} does not exist.");
            }

            _ticketRepository.DeleteTicket(ticket);
        }
    }
}
