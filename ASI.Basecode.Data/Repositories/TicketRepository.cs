using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class TicketRepository: BaseRepository, ITicketRepository
    {
        public TicketRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public IQueryable<Ticket> GetTickets()
        {
            return this.GetDbSet<Ticket>();
        }

        public bool TicketExists(int ticketId)
        {
            return this.GetDbSet<Ticket>().Any(t => t.TicketId == ticketId);
        }

        public Ticket? GetTicketById(int ticketId)
        {
            return GetDbSet<Ticket>().FirstOrDefault(t => t.TicketId == ticketId);
        }

        public void AddTicket(Ticket ticket)
        {
            this.GetDbSet<Ticket>().Add(ticket);
            SaveTicket();
        }

        public void DeleteTicket(Ticket ticket)
        {
            GetDbSet<Ticket>().Remove(ticket);
            SaveTicket();
        }

        public void SaveTicket()
        {
            UnitOfWork.SaveChanges();
        }
    }
}
