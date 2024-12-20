﻿using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
            return this.GetDbSet<Ticket>().Include(t => t.Status).Include(t => t.Category).Include(t => t.Priority);
        }

        public IQueryable<TicketCategory> GetTicketCategories()
        {
            return this.GetDbSet<TicketCategory>();
        }

        public IQueryable<TicketStatus> GetTicketStatuses()
        {
            return this.GetDbSet<TicketStatus>();
        }

        public IQueryable<TicketPriority> GetTicketPriorities()
        {
            return this.GetDbSet<TicketPriority>();
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

        public void AddTicketAttachment(Attachment attachment)
        {
            this.GetDbSet<Attachment>().Add(attachment);
        }

        public void DeleteTicketAttachment(Attachment attachment)
        {
            this.GetDbSet<Attachment>().Remove(attachment);
        }

        public void SaveTicket()
        {
            UnitOfWork.SaveChanges();
        }
    }
}
