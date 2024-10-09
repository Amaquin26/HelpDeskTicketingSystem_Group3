using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface ITicketRepository
    {

        /// <summary>
        /// Retrieves all tickets from the database.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable{Ticket}"/> representing the collection of tickets.
        /// </returns>
        IQueryable<Ticket> GetTickets();

        /// <summary>
        /// Retrieves all ticket categories from the database.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable{TicketCategory}"/> representing the collection of tickets categories.
        /// </returns>
        IQueryable<TicketCategory> GetTicketCategories();

        /// <summary>
        /// Retrieves all ticket statuses from the database.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable{TicketStatus}"/> representing the collection of ticket statuses.
        /// </returns>
        IQueryable<TicketStatus> GetTicketStatuses();

        /// <summary>
        /// Retrieves all ticket priorities from the database.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable{TicketPriority}"/> representing the collection of ticket priorities.
        /// </returns>
        IQueryable<TicketPriority> GetTicketPriorities();

        /// <summary>
        /// Checks if a ticket exists in the database by its <paramref name="ticketId"/>.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket to check.</param>
        /// <returns>
        /// <c>true</c> if the ticket exists; otherwise, <c>false</c>.
        /// </returns>
        bool TicketExists(int ticketId);

        /// <summary>
        /// Adds the new <paramref name="ticket"/> to the database.
        /// </summary>
        /// <param name="ticket">The ticket to add.</param>
        void AddTicket(Ticket ticket);

        /// <summary>
        /// Gets the ticket using the <paramref name="ticketId"/> from the database.
        /// </summary>
        /// <param name="ticketId">The ID use to retrieve a specific ticket.</param>
        Ticket? GetTicketById(int ticketId);

        /// <summary>
        /// Deletes the <paramref name="ticket"/> from the database.
        /// </summary>
        /// <param name="ticket">The ticket to delete.</param>
        void DeleteTicket(Ticket ticket);

        /// <summary>
        /// Save the tickets by calling the UnitOfWork to save changes in the database.
        /// </summary>
        /// <remarks>
        /// Useful when making changes to the retrieved tickets and is only advisable for that usage.
        /// </remarks>
        void SaveTicket();
    }
}
