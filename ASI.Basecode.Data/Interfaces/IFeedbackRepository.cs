using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    /// <summary>
    /// Defines methods for accessing and managing feedback data in the database.
    /// </summary>
    public interface IFeedbackRepository
    {
        /// <summary>
        /// Retrieves all feedback entries from the database.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable{Feedback}"/> representing the collection of feedback entries.
        /// </returns>
        IQueryable<Feedback> GetFeedbacks();

        /// <summary>
        /// Checks if a feedback entry exists in the database by its <paramref name="feedbackId"/>.
        /// </summary>
        /// <param name="feedbackId">The ID of the feedback to check.</param>
        /// <returns>
        /// <c>true</c> if the feedback exists; otherwise, <c>false</c>.
        /// </returns>
        bool FeedbackExists(int feedbackId);

        /// <summary>
        /// Adds a new <paramref name="feedback"/> entry to the database.
        /// </summary>
        /// <param name="feedback">The feedback entry to add.</param>
        void AddFeedback(Feedback feedback);

        /// <summary>
        /// Retrieves a feedback entry using the <paramref name="feedbackId"/> from the database.
        /// </summary>
        /// <param name="feedbackId">The ID used to retrieve a specific feedback entry.</param>
        /// <returns>
        /// The <see cref="Feedback"/> entry if found; otherwise, <c>null</c>.
        /// </returns>
        Feedback? GetFeedbackById(int feedbackId);

        /// <summary>
        /// Deletes the specified <paramref name="feedback"/> entry from the database.
        /// </summary>
        /// <param name="feedback">The feedback entry to delete.</param>
        void DeleteFeedback(Feedback feedback);

        /// <summary>
        /// Saves changes made to the feedback entries by calling the UnitOfWork to save changes in the database.
        /// </summary>
        /// <remarks>
        /// Useful when making changes to the retrieved feedback entries and is only advisable for that usage.
        /// </remarks>
        void SaveFeedback();
    }
}
