using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Dto;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;

namespace ASI.Basecode.Services.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFeedbackRepository _feedbackRepository;

        public TeamService(ITeamRepository teamRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, ITicketRepository ticketRepository, IFeedbackRepository feedbackRepository)
        {
            _teamRepository = teamRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _feedbackRepository = feedbackRepository;
        }

        public List<TeamViewModel> GetTeams()
        {
            var teams = _teamRepository.GetTeams().Join(_userRepository.GetUsers(),
                team => team.CreatedBy,
                user => user.UserId,
                (team, user) => new TeamViewModel
                {
                    TeamId = team.TeamId,
                    TeamName = team.TeamName,
                    TeamLeaderId = team.TeamLeaderId,
                    TeamSpecialization = team.TeamSpecialization,
                    CreatedBy = user.Name ?? "N/A",
                    CreatedTime = team.CreatedTime,
                    TeamLeaderName = team.TeamLeader.Name ?? "No Team Leader"
                })
                .ToList();

            return teams;
        }

        public PagedTeamViewModel GetListOfTeams(string searchQuery, int page, int pageSize)
        {

            PagedTeamViewModel pageTeamViewModel = new PagedTeamViewModel();

            var role = _httpContextAccessor.HttpContext?.User.FindFirst("Role")?.Value;
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var current_user = _userRepository.GetUsers().FirstOrDefault(x => x.UserId == userId);

            List<TeamViewModel> teams;

            if(role == "Admin" || role == "Super Admin")
            {
                teams = _teamRepository.GetTeams().Join(_userRepository.GetUsers(),
                team => team.CreatedBy,
                user => user.UserId,
                (team, user) => new TeamViewModel
                {
                    TeamId = team.TeamId,
                    TeamName = team.TeamName,
                    TeamLeaderId = team.TeamLeaderId,
                    TeamSpecialization = team.TeamSpecialization,
                    CreatedBy = user.Name ?? "N/A",
                    CreatedTime = team.CreatedTime,
                    TeamLeaderName = team.TeamLeader.Name ?? "No Team Leader"
                })
                .ToList();
            }
            else
            {
                teams = _teamRepository.GetTeams().Join(_userRepository.GetUsers(),
                team => team.CreatedBy,
                user => user.UserId,
                (team, user) => new TeamViewModel
                {
                    TeamId = team.TeamId,
                    TeamName = team.TeamName,
                    TeamLeaderId = team.TeamLeaderId,
                    TeamSpecialization = team.TeamSpecialization,
                    CreatedBy = user.Name ?? "N/A",
                    CreatedTime = team.CreatedTime,
                    TeamLeaderName = team.TeamLeader.Name ?? "No Team Leader"
                })
                .Where(x => x.TeamId == current_user.TeamId || x.TeamLeaderId == userId)
                .ToList();
            }


            // Filter by search query
            if (!string.IsNullOrEmpty(searchQuery))
            {
                teams = teams.Where(t => t.TeamName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                                    t.TeamLeaderName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                                    t.TeamSpecialization.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Paginate the teams
            var pagedTeams = teams.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var totalPages = (int)Math.Ceiling(teams.Count / (double)pageSize);

            pageTeamViewModel.teams = pagedTeams;
            pageTeamViewModel.totalPages = totalPages;

            return pageTeamViewModel;
        }

        public TeamViewModel GetTeamById(int id)
        {
            var team = _teamRepository.GetTeamById(id);
            if (team == null) return null;

            return new TeamViewModel
            {
                TeamId = team.TeamId,
                TeamName = team.TeamName,
                TeamLeaderId = team.TeamLeaderId,
                TeamSpecialization = team.TeamSpecialization,
                CreatedBy = team.CreatedBy,
                CreatedTime = team.CreatedTime
            };
        }

        public void AddTeam(TeamViewModel model)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var team = new Team
            {
                TeamName = model.TeamName,
                TeamLeaderId = model.TeamLeaderId,
                TeamSpecialization = model.TeamSpecialization,
                CreatedBy = userId,
                CreatedTime = DateTime.Now
            };

            _teamRepository.AddTeam(team);
        }

        public void EditTeam(TeamViewModel model)
        {
            var team = new Team
            {
                TeamId = model.TeamId,
                TeamName = model.TeamName,
                TeamLeaderId = model.TeamLeaderId,
                TeamSpecialization = model.TeamSpecialization,
                UpdatedBy = model.CreatedBy,
                UpdatedTime = DateTime.Now
            };

            _teamRepository.UpdateTeam(team);
        }

        public void DeleteTeam(int id)
        {
            _teamRepository.DeleteTeam(id);
        }

        public TeamDetailsDto? GetTeamDetails(int id, string? filter)
        {
            var teamDetails = _teamRepository.GetTeams()
                .Include(x => x.TeamLeader)
                .Where(t => t.TeamId == id)
                .Select(team => new TeamDetailsDto
                {
                    TeamId = team.TeamId,
                    Name = team.TeamName,
                    Specialization = team.TeamSpecialization,
                    TeamLeaderId = team.TeamLeaderId,
                    LeaderEmail = team.TeamLeader.Email,
                    LeaderName = team.TeamLeader.Name,
                })
                .FirstOrDefault();

            if (teamDetails == null)
            {
                return null;
            }

            var ticketQuery = _ticketRepository.GetTickets()
                .Include(t => t.Category)
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Where(t => t.TeamAssignedId == teamDetails.TeamId);
                

            // Convert filter parameter to an integer if it is not null or empty
            int? filterValue = null;
            if (!string.IsNullOrEmpty(filter) && int.TryParse(filter, out var parsedFilter))
            {
                filterValue = parsedFilter;
            }

            var tickets = ticketQuery.ToList();

            // If filter is not 0, apply the filter by StatusId
            if (filterValue.HasValue && filterValue.Value != 0)
            {
                ticketQuery = ticketQuery.Where(t => t.StatusId == filterValue.Value);
            }

            var ticketsToDisplay = ticketQuery.ToList();

            var ticketIds = tickets.Select(t => t.TicketId).ToList();

            var customerRating = _feedbackRepository.GetFeedbacks()
                .Where(t => ticketIds.Contains(t.TicketId))
                .Select(f => f.Rating)
                .ToList() // Bring data into memory
                .DefaultIfEmpty(0) // Handle empty collections
                .Average();

            double averageResolutionMinutes = tickets
             .Where(t => t.StatusId == 3 && t.ResolvedTime != null)
            .Select(t => (t.ResolvedTime.Value - t.CreatedTime).TotalMinutes) // Get resolution times in minutes
            .DefaultIfEmpty(0)
            .Average();

            int averageHours = (int)(averageResolutionMinutes / 60);
            int averageMinutes = (int)(averageResolutionMinutes % 60);

            string averageResolutionTime = $"{averageHours}h {averageMinutes}m";

            teamDetails.TicketsResolved = tickets.Where(t => t.StatusId == 3).Count();
            teamDetails.CustomerRating = (float)customerRating;
            teamDetails.AverageResolutionTimeString = averageResolutionTime;

            var ticketDtos = ticketsToDisplay.Select(t => new TicketDto
            {
                TicketId = t.TicketId,
                Title = t.Title,
                Description = t.Description,
                AssigneeId = t.AssigneeId,
                TeamAssignedId = t.TeamAssignedId,
                CreatedBy = _userRepository.GetUsers()
                        .Where(u => u.UserId == t.CreatedBy)
                        .Select(u => u.Name)
                        .FirstOrDefault(),
                AgentName = _userRepository.GetUsers()
                        .Where(u => u.UserId == t.AssigneeId)
                        .Select(u => u.Name)
                        .FirstOrDefault(),
                TeamName = _teamRepository.GetTeams()
                           .Where(te => te.TeamId == t.TeamAssignedId)
                           .Select(te => te.TeamName)
                           .FirstOrDefault(),
                StatusName = t.Status.StatusName,
                CategoryName = t.Category.CategoryName,
                PriorityName = t.Priority.PriorityName,
                CategoryId = t.CategoryId,
                StatusId = t.StatusId,
                PriorityId = t.PriorityId,
                DateAdded = t.CreatedTime
            }).ToList();

            var teamMembers = _userRepository.GetUsers()
                .Where(x => x.TeamId == id && teamDetails.TeamLeaderId != x.UserId)
                .Select(
                    x => new TeamMember
                    {
                        UserId = x.UserId,
                        Name = x.Name,
                        Role = x.Role.RoleName,
                        Email = x.Email
                    }
                )
                .ToList();

            teamDetails.Tickets = ticketDtos;
            teamDetails.TeamMebers = teamMembers;
            teamDetails.Filter = filterValue ?? 0;

            return teamDetails;
        }

        public void RemoveTeamMember(string userId)
        {
            var user = _userRepository.GetUsers().FirstOrDefault(x => x.UserId == userId);

            if (user != null)
            {
                user.TeamId = null;
                _userRepository.UpdateUser(user);   
            }
        }

        public List<User> GetAdditionalMembers(int teamId)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var team = _teamRepository.GetTeams().FirstOrDefault(x => x.TeamId == teamId);

            if(team == null)
            {
                return new List<User>();
            }

            var users = _userRepository.GetAgents()
                .Where(x => x.TeamId == null && team.TeamLeaderId != userId)
                .ToList();

            return users;
        }

        public void AddAgentToTeam(string userId, int teamId)
        {
            var user = _userRepository.GetAgents().FirstOrDefault(x => x.UserId == userId);

            if(user != null)
            {
                user.TeamId = teamId;
            }

            _userRepository.UpdateUser(user);
        }     
    }
}
