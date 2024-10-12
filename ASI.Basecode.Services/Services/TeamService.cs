using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public TeamService(ITeamRepository teamRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _teamRepository = teamRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public List<TeamViewModel> GetListOfTeams()
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
    }

}
