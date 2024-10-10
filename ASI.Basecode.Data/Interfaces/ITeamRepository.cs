using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface ITeamRepository
    {
        IQueryable<Team> GetTeams();
        Team GetTeamById(int teamId);
        void AddTeam(Team team);
        void UpdateTeam(Team team);
        void DeleteTeam(int teamId);
        void SaveTeam();
    }

}
