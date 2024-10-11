using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ASI.Basecode.Data.Repositories
{
    public class FeedbackRepository : BaseRepository, IFeedbackRepository
    {
        public FeedbackRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IQueryable<Feedback> GetFeedbacks()
        {
            return GetDbSet<Feedback>().Include(f => f.User).Include(f => f.Ticket);
        }

        public bool FeedbackExists(int feedbackId)
        {
            return GetDbSet<Feedback>().Any(f => f.FeedbackId == feedbackId);
        }

        public Feedback? GetFeedbackById(int feedbackId)
        {
            return GetDbSet<Feedback>().Include(f => f.User).Include(f => f.Ticket)
                .FirstOrDefault(f => f.FeedbackId == feedbackId);
        }

        public void AddFeedback(Feedback feedback)
        {
            GetDbSet<Feedback>().Add(feedback);
            SaveFeedback();
        }

        public void DeleteFeedback(Feedback feedback)
        {
            GetDbSet<Feedback>().Remove(feedback);
            SaveFeedback();
        }

        public void SaveFeedback()
        {
            UnitOfWork.SaveChanges();
        }
    }
}
