using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Dto;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Claims;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ITeamRepository _teamRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository repository, ITeamRepository teamRepository, ITicketRepository ticketRepository, IFeedbackRepository feedbackRepository, IHttpContextAccessor httpContextAccessor,IMapper mapper, IUserActivityRepository userActivityRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _teamRepository = teamRepository;
            _ticketRepository = ticketRepository;
            _feedbackRepository = feedbackRepository;
            _httpContextAccessor = httpContextAccessor;
            _userActivityRepository = userActivityRepository;
        }

        public LoginResult AuthenticateUser(string email, string password, ref User user)
        {
            user = new User();
            var passwordKey = PasswordManager.EncryptPassword(password);
            user = _repository.GetUsers().Include(x => x.Role).Where(x => x.Email == email &&
                                                     x.Password == passwordKey).FirstOrDefault();

            return user != null ? LoginResult.Success : LoginResult.Failed;
        }

        public void AddUser(UserViewModel model)
        {
            var user = new User();
            if (!_repository.EmailExists(model.Email))
            {
                _mapper.Map(model, user);
                user.Password = PasswordManager.EncryptPassword(model.Password);
                user.CreatedTime = DateTime.Now;
                user.UpdatedTime = DateTime.Now;
                user.CreatedBy = System.Environment.UserName;
                user.UpdatedBy = System.Environment.UserName;

                Guid userId = Guid.NewGuid();
                user.UserId = userId.ToString();
                user.RoleId = 1;
                user.Name = model.Name;
                user.IsActive = model.IsActive;
                user.Email = model.Email;
                _repository.AddUser(user);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserExists);
            }
        }
        public void AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!_repository.UserExists(user.Email)) // Check if user exists by email
            {
                // Set additional properties for the new user
                user.UserId = Guid.NewGuid().ToString();
                user.Password = PasswordManager.EncryptPassword(user.Password);
                user.CreatedTime = DateTime.Now;
                user.UpdatedTime = DateTime.Now;
                user.CreatedBy = Environment.UserName;
                user.UpdatedBy = Environment.UserName;

                _repository.AddUser(user);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserExists);
            }
        }
        public List<User> GetUsers()
        {
            return _repository.GetUsers().Include(x => x.Role).Where(u => u.IsActive).ToList();
        }
        public void DeleteUser(User user)
        {
            var existingUser = _repository.GetUsers().FirstOrDefault(u => u.UserId == user.UserId);
            if (existingUser != null)
            {
                existingUser.IsActive = user.IsActive == false;
                _repository.DeleteUser(existingUser);
            }
        }
        public void UpdateUser(User user)
        {
            var existingUser = _repository.GetUsers().FirstOrDefault(u => u.UserId == user.UserId);
            if (existingUser != null)
            {
                existingUser.Name = user.Name ?? existingUser.Name;
                existingUser.Email = user.Email ?? existingUser.Email;
                if (!string.IsNullOrEmpty(user.Password))
                {
                    existingUser.Password = PasswordManager.EncryptPassword(user.Password); // Optional: Encrypt password if necessary
                }
                existingUser.RoleId = user.RoleId;
                existingUser.UpdatedTime = DateTime.Now;
                existingUser.UpdatedBy = Environment.UserName;
                existingUser.TeamId = user.TeamId;

                _repository.UpdateUser(existingUser);
            }
        }

        public void UpdateProfile(UpdateUserViewModel user)
        {
            var existingUser = _repository.GetUsers().FirstOrDefault(u => u.UserId == user.UserId);
            if (existingUser != null)
            {
                existingUser.Name = user.Name ?? existingUser.Name;
                existingUser.Email = user.Email ?? existingUser.Email;

                if (existingUser.Email != user.Email) 
                {
                    if (_repository.EmailExists(user.Email))
                    {
                        throw new Exception("Email already exist");
                    }
                }

                if (!string.IsNullOrEmpty(user.Password))
                {
                    existingUser.Password = PasswordManager.EncryptPassword(user.Password); // Optional: Encrypt password if necessary
                }
                existingUser.UpdatedTime = DateTime.Now;
                existingUser.UpdatedBy = Environment.UserName;

                _repository.UpdateUser(existingUser);
            }
        }

        public IQueryable<User> GetAgents()
        {
            return _repository.GetAgents(); // Call the repository method to get agents
        }

        public IQueryable<User> GetTeamLeaders()
        {
            return _repository.GetTeamLeaders(); // Call the repository method to get team leaders
        }

        public UserDetailsDto? GetUserDetails(string id, string? filter)
        {
            var userDetails = new UserDetailsDto();

            var user = _repository.GetUsers()
                .Include(u => u.Role) // Eagerly load Role
                .Where(u => u.IsActive && u.UserId == id)
                .Select(user => new UserDetailsDto
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    RoleName = user.Role.RoleName,
                    TeamName = _teamRepository.GetTeams()
                        .Where(t => t.TeamId == user.TeamId)
                        .Select(t => t.TeamName)
                        .FirstOrDefault() // Query for TeamName separately
                })
                .FirstOrDefault();

            if (userDetails == null)
            {
                return null;
            }

            var ticketQuery = _ticketRepository.GetTickets()
                .Include(t => t.Category)
                .Include(t => t.Status)
                .Include(t => t.Priority);

            // Convert filter parameter to an integer if it is not null or empty
            int? filterValue = null;
            if (!string.IsNullOrEmpty(filter) && int.TryParse(filter, out var parsedFilter))
            {
                filterValue = parsedFilter;
            }

            // If filter is not 0, apply the filter by StatusId
            if (filterValue.HasValue && filterValue.Value != 0)
            {
                ticketQuery = ticketQuery.Where(t => t.StatusId == filterValue.Value);
            }

            if (user.RoleName == "User")
            {
                // if the user is just a user role, retrieved the submitted tickets
                ticketQuery = ticketQuery.Where(t => t.CreatedBy == user.UserId);
            }
            else
            {
                // retrieved tickets assigned for non user roles
                ticketQuery = ticketQuery.Where(t => t.AssigneeId == user.UserId || t.TeamAssignedId == user.TeamId);
            }

            var tickets = ticketQuery.ToList();

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

            userDetails.UserId = user.UserId;
            userDetails.Name = user.Name;
            userDetails.Email = user.Email;
            userDetails.RoleName = user.RoleName;
            userDetails.TeamName = user.TeamName;
            userDetails.TicketsResolved = tickets.Where(t => t.StatusId == 3).Count();
            userDetails.CustomerRating = (float)customerRating;
            userDetails.AverageResolutionTimeString = averageResolutionTime;

            var ticketDtos = tickets.Select(t => new TicketDto
            {
                TicketId = t.TicketId,
                Title = t.Title,
                Description = t.Description,
                AssigneeId = t.AssigneeId,
                TeamAssignedId = t.TeamAssignedId,
                CreatedBy = _repository.GetUsers()
                        .Where(u => u.UserId == t.CreatedBy)
                        .Select(u => u.Name)
                        .FirstOrDefault(),
                AgentName = _repository.GetUsers()
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

            userDetails.Tickets = ticketDtos;
            userDetails.Filter = filterValue ?? 0;

            return userDetails;
        }

        public User? GetMyProfile()
        {
            var id = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return _repository.GetUsers().FirstOrDefault(x => x.UserId == id);
        }

        public Role GetUserRole(string userId)
        {
            return _repository.GetUserRole(userId);
        }

        public List<Role> GetUserRoles()
        {
            var role = _httpContextAccessor.HttpContext?.User.FindFirst("Role")?.Value;
            var rolesQuery = _repository.GetUserRoles();

            if (role == "Admin")
            {
                var roleIds = new List<int> { 3, 4 }; // admins can only add users or agents
                rolesQuery = rolesQuery.Where(x => roleIds.Contains(x.RoleId));
            }

            return rolesQuery.ToList();
        }

        public (bool,bool) GetUserPreference()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = _repository.GetUsers().FirstOrDefault(x => x.UserId == userId); 

            if (user == null)
            {
                return (false,false);
            }

            return (user.ReceiveNotifications, user.TicketViewMode);
        }

        public void EditUserPreference(bool ReceiveNotification, bool TicketViewMode)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = _repository.GetUsers().FirstOrDefault(x => x.UserId == userId);

            if(user != null)
            {
                user.ReceiveNotifications = ReceiveNotification;
                user.TicketViewMode = TicketViewMode;
            }

            _repository.UpdateUser(user);
        }

        public List<UserActivity> GetUserActivity(string userId)
        {
            var userActivity = _userActivityRepository.GetUserActivities().Where(x => x.UserId == userId);

            return userActivity.ToList();
        }

        public List<UserActivity> GetMyUserActivity()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userActivity = _userActivityRepository.GetUserActivities().Where(x => x.UserId == userId);

            return userActivity.ToList();
        }

        public void DeleteUserActivity(int activityId)
        {
            var activity = _userActivityRepository.GetUserActivities().FirstOrDefault(x => x.Id == activityId);

            if(activity != null)
            {
                _userActivityRepository.DeleteUserActivity(activity);
            }
        }
    }
}
