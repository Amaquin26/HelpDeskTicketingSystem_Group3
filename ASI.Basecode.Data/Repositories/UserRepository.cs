using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ASI.Basecode.Data.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork) 
        {
        }

        public IQueryable<User> GetUsers()
        {
            // Return only active users
            return this.GetDbSet<User>().Include(u => u.Role).Where(u => u.IsActive);
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
            // Return only active agents with RoleId 3
            return this.GetDbSet<User>().Where(x => x.RoleId == 3 && x.IsActive);
        }

        public void AddUser(User user)
        {
            this.GetDbSet<User>().Add(user);
            UnitOfWork.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            if (user != null)
            {
                this.GetDbSet<User>().Update(user);
                UnitOfWork.SaveChanges();
            }
        }

        public void DeleteUser(User user)
        {
            if (user != null)
            {
                user.IsActive = false; // Soft delete by marking as inactive
                UpdateUser(user); // Update the user record
                UnitOfWork.SaveChanges();
            }
        }
    }
}
