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
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamRepository"/> class with the specified <see cref="IUnitOfWork"/>.
        /// </summary>
        /// <param name="unitOfWork">The unit of work to be used for saving changes.</param>
        public TeamRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Retrieves all teams, including their team leaders and users.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable{Team}"/> representing the collection of all teams, 
        /// including related team leader and user information.
        /// </returns>
        public IQueryable<Team> GetTeams()
        {
            return GetDbSet<Team>()
                .Include(t => t.TeamLeader)
                .Include(t => t.Users);
        }

        /// <summary>
        /// Retrieves a team by its unique identifier, including team leader and users.
        /// </summary>
        /// <param name="teamId">The unique identifier of the team.</param>
        /// <returns>
        /// A <see cref="Team"/> object representing the team with the specified <paramref name="teamId"/>.
        /// Returns <c>null</c> if the team is not found.
        /// </returns>
        public Team GetTeamById(int teamId)
        {
            return GetDbSet<Team>()
                .Include(t => t.TeamLeader)
                .Include(t => t.Users)
                .FirstOrDefault(t => t.TeamId == teamId);
        }

        /// <summary>
        /// Adds a new team to the database.
        /// </summary>
        /// <param name="team">The <see cref="Team"/> object representing the new team to be added.</param>
        public void AddTeam(Team team)
        {
            GetDbSet<Team>().Add(team);
            SaveTeam();
        }

        /// <summary>
        /// Updates the details of an existing team.
        /// </summary>
        /// <param name="team">The <see cref="Team"/> object containing updated information.</param>
        /// <exception cref="Exception">Thrown if the team is not found in the database.</exception>
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

        /// <summary>
        /// Deletes a team by its unique identifier.
        /// </summary>
        /// <param name="teamId">The unique identifier of the team to be deleted.</param>
        /// <exception cref="Exception">Thrown if the team is not found in the database.</exception>
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

        /// <summary>
        /// Saves all changes made to the repository.
        /// </summary>
        public void SaveTeam()
        {
            UnitOfWork.SaveChanges();
        }
    }
}

