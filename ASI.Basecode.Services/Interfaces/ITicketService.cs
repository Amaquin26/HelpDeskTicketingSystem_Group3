using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Dto;
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
        List<TicketDto> GetListOfTickets();

        /// <summary>
        /// Retrieves a list of all ticket categories from the database.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TicketCategory"/> objects. 
        /// If no ticket categories exist, an empty list will be returned.
        /// </returns>
        List<TicketCategory> GetTicketCategoryList();

        /// <summary>
        /// Retrieves a list of all ticket statuses from the database.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TicketStatus"/> objects. 
        /// If no ticket statuses exist, an empty list will be returned.
        /// </returns>
        List<TicketStatus> GetTicketStatusList();

        /// <summary>
        /// Retrieves a list of all ticket priorities from the database.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TicketPriority"/> objects. 
        /// If no ticket priorities exist, an empty list will be returned.
        /// </returns>
        List<TicketPriority> GetTicketPriorityList();

        /// <summary>
        /// Adds a new ticket based on the provided <see cref="TicketDto"/>.
        /// </summary>
        /// <param name="model">The ticket view model containing the details of the ticket to be added.</param>
        /// <remarks>
        /// This method validates the provided ticket model, creates a new <see cref="Ticket"/>
        /// entity, and calls the repository to save it in the database.
        /// </remarks>
        void AddTicket(TicketFormModel model);

        /// <summary>
        /// Edits an existing ticket based on the provided <see cref="TicketDto"/>.
        /// </summary>
        /// <param name="model">The view model containing updated ticket information.</param>
        /// <remarks>
        /// The method retrieves the ticket by its ID, updates its properties with the data from the view model, 
        /// and saves the changes to the database.
        /// </remarks>
        /// <exception cref="KeyNotFoundException">Thrown when the ticket with the specified ID does not exist.</exception>
        void EditTicket(TicketFormModel model);

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
