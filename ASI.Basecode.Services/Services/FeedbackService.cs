using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ASI.Basecode.Data.Repositories;

namespace ASI.Basecode.Services.Services
{
    /// <summary>
    /// Provides methods to manage feedback operations.
    /// </summary>
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackService"/> class.
        /// </summary>
        /// <param name="feedbackRepository">The feedback repository instance used for data access.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor instance for user information.</param>
        public FeedbackService(IFeedbackRepository feedbackRepository, IHttpContextAccessor httpContextAccessor)
        {
            _feedbackRepository = feedbackRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Retrieves a list of feedback entries.
        /// </summary>
        /// <returns>
        /// A list of <see cref="FeedbackViewModel"/> representing the feedback entries.
        /// </returns>
        public List<FeedbackViewModel> GetListOfFeedbacks()
        {
            return _feedbackRepository.GetFeedbacks()
                .Select(f => new FeedbackViewModel
                {
                    FeedbackId = f.FeedbackId,
                    TicketId = f.TicketId,
                    UserId = f.UserId,
                    Comment = f.Comment,
                    Rating = f.Rating,
                    CreatedBy = f.User.Name,
                    CreatedTime = f.CreatedTime,
                    TicketTitle = f.Ticket.Title
                }).ToList();
        }

        /// <summary>
        /// Adds a new feedback entry based on the provided model.
        /// </summary>
        /// <param name="model">The feedback model to add.</param>
        public void AddFeedback(FeedbackViewModel model)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var feedback = new Feedback
            {
                TicketId = model.TicketId,
                UserId = userId,
                Comment = model.Comment,
                Rating = model.Rating,
                CreatedTime = DateTime.UtcNow
            };
            _feedbackRepository.AddFeedback(feedback);
        }

        /// <summary>
        /// Edits an existing feedback entry based on the provided model.
        /// </summary>
        /// <param name="model">The feedback model containing updated information.</param>
        public void EditFeedback(FeedbackViewModel model)
        {
            var feedback = _feedbackRepository.GetFeedbackById(model.FeedbackId);
            if (feedback != null)
            {
                feedback.Comment = model.Comment;
                feedback.Rating = model.Rating;
                _feedbackRepository.SaveFeedback();
            }
        }

        /// <summary>
        /// Deletes a feedback entry by its ID.
        /// </summary>
        /// <param name="feedbackId">The ID of the feedback entry to delete.</param>
        public void DeleteFeedback(int feedbackId)
        {
            var feedback = _feedbackRepository.GetFeedbackById(feedbackId);
            if (feedback != null)
            {
                _feedbackRepository.DeleteFeedback(feedback);
            }
        }
    }
}
