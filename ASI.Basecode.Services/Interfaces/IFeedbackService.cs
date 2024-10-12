using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    /// <summary>
    /// Defines methods for managing feedback operations.
    /// </summary>
    public interface IFeedbackService
    {
        /// <summary>
        /// Retrieves a list of feedback entries.
        /// </summary>
        /// <returns>
        /// A list of <see cref="FeedbackViewModel"/> representing the feedback entries.
        /// </returns>
        List<FeedbackViewModel> GetListOfFeedbacks();

        /// <summary>
        /// Adds a new feedback entry based on the provided model.
        /// </summary>
        /// <param name="model">The feedback model to add.</param>
        void AddFeedback(FeedbackViewModel model);

        /// <summary>
        /// Edits an existing feedback entry based on the provided model.
        /// </summary>
        /// <param name="model">The feedback model containing updated information.</param>
        void EditFeedback(FeedbackViewModel model);

        /// <summary>
        /// Deletes a feedback entry by its ID.
        /// </summary>
        /// <param name="feedbackId">The ID of the feedback entry to delete.</param>
        void DeleteFeedback(int feedbackId);
    }
}
