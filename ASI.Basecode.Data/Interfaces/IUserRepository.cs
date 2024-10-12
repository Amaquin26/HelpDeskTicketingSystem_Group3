using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Gets a collection of all users.
        /// </summary>
        IEnumerable<object> Users { get; }

        /// <summary>
        /// Retrieves a queryable collection of all users.
        /// </summary>
        /// <returns>A queryable list of users.</returns>
        IQueryable<User> GetUsers();

        /// <summary>
        /// Checks if a user exists based on the provided user ID.
        /// </summary>
        /// <param name="userId">The unique ID of the user.</param>
        /// <returns>True if the user exists, otherwise false.</returns>
        bool UserExists(string userId);

        /// <summary>
        /// Checks if an email address is already associated with an existing user.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns>True if the email exists, otherwise false.</returns>
        bool EmailExists(string email);

        /// <summary>
        /// Adds a new user to the repository.
        /// </summary>
        /// <param name="user">The user object to add.</param>
        void AddUser(User user);

        /// <summary>
        /// Updates an existing user in the repository.
        /// </summary>
        /// <param name="user">The user object with updated data.</param>
        void UpdateUser(User user);

        /// <summary>
        /// Soft deletes a user by marking them as inactive.
        /// </summary>
        /// <param name="user">The user object to delete.</param>
        void DeleteUser(User user);

        /// <summary>
        /// Retrieves a queryable collection of users with the "Agent" role.
        /// </summary>
        /// <returns>A queryable list of agent users.</returns>
        IQueryable<User> GetAgents();

        /// <summary>
        /// Retrieves a queryable collection of users with the "Team Leader" role.
        /// </summary>
        /// <returns>A queryable list of team leader users.</returns>
        IQueryable<User> GetTeamLeaders();
    }
}
