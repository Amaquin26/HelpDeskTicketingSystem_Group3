using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Dto;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using EFCore.BulkExtensions.SqlAdapters;
using LinqKit;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Claims;
namespace ASI.Basecode.Services.Services
{
    public class TicketService:ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationRepository _notificationRepository;

        public TicketService(ITicketRepository ticketRepository, IUserRepository userRepository, ITeamRepository teamRepository, IHttpContextAccessor httpContextAccessor, INotificationRepository notificationRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _teamRepository = teamRepository;
            _httpContextAccessor = httpContextAccessor;
            _notificationRepository = notificationRepository;
        }

        public List<TicketDto> GetListOfTickets()
        {
            var role = _httpContextAccessor.HttpContext?.User.FindFirst("Role")?.Value;
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var ticketsQuery = _ticketRepository.GetTickets();

            // If not super admin or an admin, only display the tickets that assigned or was created by that user
            if(!(role == "Super Admin" || role == "Admin"))
            {
                ticketsQuery = ticketsQuery.Where(x => x.CreatedBy == userId || x.AssigneeId == userId);
            }

            var tickets = ticketsQuery.Join(_userRepository.GetUsers(),
                ticket => ticket.CreatedBy,
                user => user.UserId,
                (ticket, user) => new TicketDto
                {
                    TicketId = ticket.TicketId,
                    Title = ticket.Title,
                    Description = ticket.Description,
                    TeamAssignedId = ticket.TeamAssignedId,
                    AssigneeId = ticket.AssigneeId,
                    CreatedBy = user.Name,
                    CreatedById = ticket.CreatedBy,
                    StatusName = ticket.Status.StatusName,
                    CategoryName = ticket.Category.CategoryName,
                    PriorityName = ticket.Priority.PriorityName,
                    DateAdded = ticket.CreatedTime,
                    CategoryId = ticket.CategoryId,
                    StatusId = ticket.StatusId,
                    PriorityId = ticket.PriorityId,
                    CanEdit = ticket.CreatedBy == userId || ticket.AssigneeId == userId || (role == "Super Admin" || role == "Admin")
                })
            .ToList();

            return tickets;
        }

        public (TicketDto, bool) GetTicketById(int ticketId)
        {
            var role = _httpContextAccessor.HttpContext?.User.FindFirst("Role")?.Value;
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var ticket = (from t in _ticketRepository.GetTickets()
                join u in _userRepository.GetUsers()
                    on t.CreatedBy equals u.UserId into createdUsers
                from createdUser in createdUsers.DefaultIfEmpty()

                join a in _userRepository.GetUsers()
                    on t.AssigneeId equals a.UserId into assignedAgents
                from assignedAgent in assignedAgents.DefaultIfEmpty()

                join te in _teamRepository.GetTeams()
                    on t.TeamAssignedId equals te.TeamId into assignedTeams
                from assignedTeam in assignedTeams.DefaultIfEmpty()

                where t.TicketId == ticketId
                select new TicketDto
                {
                    TicketId = t.TicketId,
                    Title = t.Title,
                    Description = t.Description,
                    TeamAssignedId = t.TeamAssignedId,
                    AssigneeId = t.AssigneeId,
                    CreatedBy = createdUser.Name,
                    StatusName = t.Status.StatusName,
                    CategoryName = t.Category.CategoryName,
                    PriorityName = t.Priority.PriorityName,
                    DateAdded = t.CreatedTime,
                    CategoryId = t.CategoryId,
                    StatusId = t.StatusId,
                    PriorityId = t.PriorityId,
                    TeamName = assignedTeam.TeamName,
                    AgentName = assignedAgent.Name,
                    CanEdit = t.CreatedBy == userId || t.AssigneeId == userId || (role == "Super Admin" || role == "Admin")
                })
            .FirstOrDefault();

            if (ticket == null)
            {
                return (null, false);
            }

            return (ticket, true);
        }

        public List<TicketCategory> GetTicketCategoryList()
        {
            return _ticketRepository.GetTicketCategories().ToList();
        }

        public List<TicketStatus> GetTicketStatusList()
        {
            return _ticketRepository.GetTicketStatuses().ToList();
        }

        public List<TicketPriority> GetTicketPriorityList()
        {
            return _ticketRepository.GetTicketPriorities().ToList();
        }

        public async void AddTicket(TicketFormModel model)
        {
            var ticket = new Ticket();

            ticket.Title = model.Title;
            ticket.Description = model.Description;
            ticket.AssigneeId = model.AssigneeId;
            ticket.TeamAssignedId = model.TeamAssignedId;
            ticket.StatusId = model.StatusId;
            ticket.PriorityId = model.PriorityId;
            ticket.CategoryId = model.CategoryId;
            ticket.CreatedTime = DateTime.Now;

            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ticket.CreatedBy = userId;

            _ticketRepository.AddTicket(ticket);

            // add files
            //if (model.Files != null && model.Files.Count > 0)
            //{
            //    // Ensure the directory exists
            //    if (!Directory.Exists("UploadedFiles"))
            //    {
            //        Directory.CreateDirectory("UploadedFiles");
            //    }

            //    foreach (var file in model.Files)
            //    {
            //        if (file.Length > 0)
            //        {
            //            // Save the file
            //            var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            //            var filePath = Path.Combine("UploadedFiles", fileName);
            //            using (var stream = new FileStream(filePath, FileMode.Create))
            //            {
            //                await file.CopyToAsync(stream);
            //            }

            //            // Save file info to the database
            //            var attachment = new Attachment
            //            {
            //                FileName = fileName,
            //                FilePath = filePath,
            //                TicketId = model.TicketId
            //            };

            //            _ticketRepository.AddTicketAttachment(attachment);
            //        }
            //    }
            //}

            _ticketRepository.SaveTicket();
        }

        public void EditTicket(TicketFormModel model)
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
            ticket.UpdatedTime = DateTime.Now;

            if (model.StatusId == 3)
            {
                ticket.ResolvedTime = DateTime.Now;
            }

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
