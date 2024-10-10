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
        IEnumerable<object> Users { get; }

        IQueryable<User> GetUsers();
        bool UserExists(string userId);
        bool EmailExists(string email);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        IQueryable<User> GetAgents();
    }
}
