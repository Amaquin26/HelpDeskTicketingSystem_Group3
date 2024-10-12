using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ASI.Basecode.Data.Repositories
{
    /// <summary>
    /// Provides methods to access and manage feedback data in the database.
    /// </summary>
    public class FeedbackRepository : BaseRepository, IFeedbackRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work instance used to manage database operations.</param>
        public FeedbackRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Retrieves all feedback entries from the database.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable{Feedback}"/> representing the collection of feedback entries.
        /// </returns>
        public IQueryable<Feedback> GetFeedbacks()
        {
            return GetDbSet<Feedback>().Include(f => f.User).Include(f => f.Ticket);
        }

        /// <summary>
        /// Checks if a feedback entry exists in the database by its <paramref name="feedbackId"/>.
        /// </summary>
        /// <param name="feedbackId">The ID of the feedback to check.</param>
        /// <returns>
        /// <c>true</c> if the feedback exists; otherwise, <c>false</c>.
        /// </returns>
        public bool FeedbackExists(int feedbackId)
        {
            return GetDbSet<Feedback>().Any(f => f.FeedbackId == feedbackId);
        }

        /// <summary>
        /// Retrieves a feedback entry using the <paramref name="feedbackId"/> from the database.
        /// </summary>
        /// <param name="feedbackId">The ID used to retrieve a specific feedback entry.</param>
        /// <returns>
        /// The <see cref="Feedback"/> entry if found; otherwise, <c>null</c>.
        /// </returns>
        public Feedback? GetFeedbackById(int feedbackId)
        {
            return GetDbSet<Feedback>().Include(f => f.User).Include(f => f.Ticket)
                .FirstOrDefault(f => f.FeedbackId == feedbackId);
        }

        /// <summary>
        /// Adds a new <paramref name="feedback"/> entry to the database.
        /// </summary>
        /// <param name="feedback">The feedback entry to add.</param>
        public void AddFeedback(Feedback feedback)
        {
            GetDbSet<Feedback>().Add(feedback);
            SaveFeedback();
        }

        /// <summary>
        /// Deletes the specified <paramref name="feedback"/> entry from the database.
        /// </summary>
        /// <param name="feedback">The feedback entry to delete.</param>
        public void DeleteFeedback(Feedback feedback)
        {
            GetDbSet<Feedback>().Remove(feedback);
            SaveFeedback();
        }

        /// <summary>
        /// Saves changes made to the feedback entries by calling the UnitOfWork to save changes in the database.
        /// </summary>
        public void SaveFeedback()
        {
            UnitOfWork.SaveChanges();
        }
    }
}
