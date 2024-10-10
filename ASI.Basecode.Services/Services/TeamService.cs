using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public List<TeamViewModel> GetListOfTeams()
        {
            var teams = _teamRepository.GetTeams();
            return teams.Select(t => new TeamViewModel
            {
                TeamId = t.TeamId,
                TeamName = t.TeamName,
                TeamLeaderId = t.TeamLeaderId,
                TeamSpecialization = t.TeamSpecialization,
                CreatedBy = t.CreatedBy,
                CreatedTime = t.CreatedTime
            }).ToList();
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
            var team = new Team
            {
                TeamName = model.TeamName,
                TeamLeaderId = model.TeamLeaderId,
                TeamSpecialization = model.TeamSpecialization,
                CreatedBy = model.CreatedBy,
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
