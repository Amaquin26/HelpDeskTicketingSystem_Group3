using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class TeamRepository : BaseRepository, ITeamRepository
    {
        public TeamRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IQueryable<Team> GetTeams()
        {
            return GetDbSet<Team>()
                .Include(t => t.TeamLeader)
                .Include(t => t.Users);
        }

        public Team GetTeamById(int teamId)
        {
            return GetDbSet<Team>()
                .Include(t => t.TeamLeader)
                .Include(t => t.Users)
                .FirstOrDefault(t => t.TeamId == teamId);
        }

        public void AddTeam(Team team)
        {
            GetDbSet<Team>().Add(team);
            SaveTeam();
        }

        public void UpdateTeam(Team team)
        {
            var existingTeam = GetTeamById(team.TeamId);
            if (existingTeam != null)
            {
                existingTeam.TeamName = team.TeamName;
                existingTeam.TeamLeaderId = team.TeamLeaderId;
                existingTeam.TeamSpecialization = team.TeamSpecialization;
                existingTeam.UpdatedBy = team.UpdatedBy;
                existingTeam.UpdatedTime = DateTime.Now;

                SaveTeam();
            }
            else
            {
                throw new Exception("Team not found.");
            }
        }

        public void DeleteTeam(int teamId)
        {
            var team = GetTeamById(teamId);
            if (team != null)
            {
                GetDbSet<Team>().Remove(team);
                SaveTeam();
            }
            else
            {
                throw new Exception("Team not found.");
            }
        }

        public void SaveTeam()
        {
            UnitOfWork.SaveChanges();
        }
    }

}
