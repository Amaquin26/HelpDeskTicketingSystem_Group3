using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System.Linq;

namespace ASI.Basecode.Data.Repositories
{
    public class UserActivityRepository : BaseRepository, IUserActivityRepository
    {
        public UserActivityRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IQueryable<UserActivity> GetUserActivities()
        {
            return GetDbSet<UserActivity>();
        }

        public void AddUserActivity(UserActivity activity)
        {
            GetDbSet<UserActivity>().Add(activity);
            SaveActivity();
        }

        public void DeleteUserActivity(UserActivity activity)
        {
            GetDbSet<UserActivity>().Remove(activity);
            SaveActivity();
        }

        public void SaveActivity()
        {
            UnitOfWork.SaveChanges();
        }
    }
}
