using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Dto;
using ASI.Basecode.Services.ServiceModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Authenticates a user using the provided user ID and password.
        /// </summary>
        /// <param name="userid">The unique ID of the user.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="user">A reference to the authenticated user object.</param>
        /// <returns>A <see cref="LoginResult"/> indicating the outcome of the authentication.</returns>
        LoginResult AuthenticateUser(string userid, string password, ref User user);

        /// <summary>
        /// Adds a new user using the provided <see cref="UserViewModel"/> data.
        /// </summary>
        /// <param name="model">The user view model containing the data for the new user.</param>
        void AddUser(UserViewModel model);

        /// <summary>
        /// Retrieves a list of all users.
        /// </summary>
        /// <returns>A list of users.</returns>
        List<User> GetUsers();

        /// <summary>
        /// Retrieves a queryable collection of users, optionally filtered to only include agents.
        /// </summary>
        /// <param name="onlyAgents">If true, only returns users with the "Agent" role.</param>
        /// <returns>A queryable list of users based on the filter.</returns>
        IQueryable<User> GetUser(bool onlyAgents);

        /// <summary>
        /// Adds a new user to the system.
        /// </summary>
        /// <param name="user">The user object to add.</param>
        void AddUser(User user);

        /// <summary>
        /// Updates an existing user in the system.
        /// </summary>
        /// <param name="user">The user object with updated data.</param>
        void UpdateUser(User user);

        /// <summary>
        /// Deletes a user from the system.
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
        UserDetailsDto? GetUserDetails(string id);
        User? GetMyProfile();
        void UpdateProfile(UpdateUserViewModel user);
    }
}
