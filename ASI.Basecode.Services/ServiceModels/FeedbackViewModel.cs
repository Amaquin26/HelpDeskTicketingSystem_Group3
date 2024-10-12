using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASI.Basecode.Services.ServiceModels
{
    /// <summary>
    /// Represents a view model for feedback data associated with tickets.
    /// </summary>
    public class FeedbackViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the feedback entry.
        /// </summary>
        public int FeedbackId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated ticket.
        /// </summary>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user who provided the feedback.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the comment provided by the user.
        /// Required field with a validation error message.
        /// </summary>
        [Required(ErrorMessage = "Comment is required.")]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the rating given by the user.
        /// Required field with a validation error message.
        /// </summary>
        [Required(ErrorMessage = "Rating is required.")]
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the name of the user who created the feedback entry.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the time when the feedback was created.
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Gets or sets the title of the associated ticket.
        /// </summary>
        public string TicketTitle { get; set; }

        /// <summary>
        /// Gets or sets the list of tickets associated with the feedback.
        /// </summary>
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
