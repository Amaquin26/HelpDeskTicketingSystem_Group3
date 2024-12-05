using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.Data.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public IEnumerable<object> Users => throw new System.NotImplementedException();

        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork) 
        {
        } 

        public IQueryable<User> GetUsers()
        {
            // Return only active users
            return this.GetDbSet<User>().Include(u => u.Role).Where(u => u.IsActive);
        }

        public IQueryable<User> GetTeamLeaders()
        {
            // Return only active team leader
            return this.GetDbSet<Team>().Include(t => t.TeamLeader).Select(u => u.TeamLeader).Where(u => u.IsActive);
        }

        public bool UserExists(string userId)
        {
            return this.GetDbSet<User>().Any(x => x.UserId == userId);
        }

        public bool EmailExists(string email)
        {
            return this.GetDbSet<User>().Any(x => x.Email == email);
        }

        public IQueryable<User> GetAgents()
        {
            // Return only active agents 
            return this.GetDbSet<User>().Include(u => u.Role).Where(u => u.IsActive && u.RoleId == 3);
        }

        public void AddUser(User user)
        {
            this.GetDbSet<User>().Add(user);
            UnitOfWork.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            this.GetDbSet<User>().Update(user);
            UnitOfWork.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            this.GetDbSet<User>().Update(user);
            UnitOfWork.SaveChanges();
        }

        public Role GetUserRole(string userId)
        {
            return this.GetDbSet<User>().Include(u => u.Role).Where(u => u.UserId == userId).Select(u => u.Role).FirstOrDefault();
        }

        public IQueryable<Role> GetUserRoles()
        {
            return this.GetDbSet<Role>();
        }
    }
}
