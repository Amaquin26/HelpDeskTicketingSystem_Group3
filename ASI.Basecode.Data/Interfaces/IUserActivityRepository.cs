using ASI.Basecode.Data.Models;
using System.Linq;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IUserActivityRepository
    {
        IQueryable<UserActivity> GetUserActivities();
        void AddUserActivity(UserActivity activity);
        void DeleteUserActivity(UserActivity activity);
        void SaveActivity();
    }
}
