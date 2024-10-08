using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface ITicketService
    {
        /// <summary>
        /// Retrieves a list of all tickets from the database.
        /// </summary>
        /// <returns>
        /// A list of <see cref="Ticket"/> objects. 
        /// If no tickets exist, an empty list will be returned.
        /// </returns>
        List<Ticket> GetTickets();

        /// <summary>
        /// Adds a new ticket based on the provided <see cref="TicketViewModel"/>.
        /// </summary>
        /// <param name="model">The ticket view model containing the details of the ticket to be added.</param>
        /// <remarks>
        /// This method validates the provided ticket model, creates a new <see cref="Ticket"/>
        /// entity, and calls the repository to save it in the database.
        /// </remarks>
        void AddTicket(TicketViewModel model);

        /// <summary>
        /// Edits an existing ticket based on the provided <see cref="TicketViewModel"/>.
        /// </summary>
        /// <param name="model">The view model containing updated ticket information.</param>
        /// <remarks>
        /// The method retrieves the ticket by its ID, updates its properties with the data from the view model, 
        /// and saves the changes to the database.
        /// </remarks>
        /// <exception cref="KeyNotFoundException">Thrown when the ticket with the specified ID does not exist.</exception>
        void EditTicket(TicketViewModel model);

        /// <summary>
        /// Deletes a ticket identified by the specified ticket ID.
        /// This method retrieves the ticket from the repository and marks it for deletion.
        /// If the ticket does not exist, a <see cref="KeyNotFoundException"/> is thrown.
        /// </summary>
        /// <param name="ticketId">The unique identifier of the ticket to be deleted.</param>
        /// <exception cref="KeyNotFoundException">Thrown when no ticket with the specified ID exists.</exception>
        void DeleteTicket(int ticketId);
    }
}
