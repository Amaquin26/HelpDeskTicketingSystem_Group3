using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
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
        private readonly ITicketRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TicketService(ITicketRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Ticket> GetTickets()
        {
            return _repository.GetTickets().ToList();
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

            _repository.AddTicket(ticket);
        }

        public void EditTicket(TicketViewModel model)
        {

            var ticket = _repository.GetTicketById(model.TicketId);

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

            _repository.SaveTicket();
        }

        public void DeleteTicket(int ticketId) 
        {
            var ticket = _repository.GetTicketById(ticketId);

            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {ticketId} does not exist.");
            }

            _repository.DeleteTicket(ticket);
        }
    }
}
