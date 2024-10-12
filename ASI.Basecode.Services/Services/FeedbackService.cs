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
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FeedbackService(IFeedbackRepository feedbackRepository, IHttpContextAccessor httpContextAccessor)
        {
            _feedbackRepository = feedbackRepository;
            _httpContextAccessor = httpContextAccessor;
        }

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

        public void EditFeedback(FeedbackViewModel model)
        {
            var feedback = _feedbackRepository.GetFeedbackById(model.FeedbackId);
            if (feedback == null)
            {
                throw new KeyNotFoundException($"Feedback with ID {model.FeedbackId} does not exist.");
            }

            feedback.TicketId = model.TicketId;
            feedback.Comment = model.Comment;
            feedback.Rating = model.Rating;

            _feedbackRepository.SaveFeedback();
        }

        public void DeleteFeedback(int feedbackId)
        {
            var feedback = _feedbackRepository.GetFeedbackById(feedbackId);
            if (feedback == null)
            {
                throw new KeyNotFoundException($"Feedback with ID {feedbackId} does not exist.");
            }

            _feedbackRepository.DeleteFeedback(feedback);
        }
    }
}
