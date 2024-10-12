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
        /// <summary>
        /// Retrieves all teams from the database.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable{Team}"/> representing the collection of all teams.
        /// </returns>
        IQueryable<Team> GetTeams();

        /// <summary>
        /// Retrieves a specific team by its unique identifier.
        /// </summary>
        /// <param name="teamId">The unique identifier of the team.</param>
        /// <returns>
        /// A <see cref="Team"/> object representing the team with the specified <paramref name="teamId"/>.
        /// Returns <c>null</c> if the team is not found.
        /// </returns>
        Team GetTeamById(int teamId);

        /// <summary>
        /// Adds a new team to the database.
        /// </summary>
        /// <param name="team">The <see cref="Team"/> object representing the new team to be added.</param>
        void AddTeam(Team team);

        /// <summary>
        /// Updates the details of an existing team.
        /// </summary>
        /// <param name="team">The <see cref="Team"/> object containing updated information.</param>
        void UpdateTeam(Team team);

        /// <summary>
        /// Deletes a team from the database.
        /// </summary>
        /// <param name="teamId">The unique identifier of the team to be deleted.</param>
        void DeleteTeam(int teamId);

        /// <summary>
        /// Saves all changes made to the repository.
        /// </summary>
        void SaveTeam();
    }
}

