using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data
{
    public partial class AsiBasecodeDBContext : DbContext
    {
        public AsiBasecodeDBContext()
        {
        }

        public AsiBasecodeDBContext(DbContextOptions<AsiBasecodeDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Announcement> Announcements { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketCategory> TicketCategories { get; set; }
        public virtual DbSet<TicketStatus> TicketStatuses { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserId, "UQ__Users__1788CC4D5F4A160F")
                    .IsUnique();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });           

            // User and Role Relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)                            
                .WithMany(r => r.Users)                       
                .HasForeignKey(u => u.RoleId)                  
                .OnDelete(DeleteBehavior.Restrict);

            // Ticket and User Relationship (Assignee)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Assignee)                        
                .WithMany(u => u.Tickets)                      
                .HasForeignKey(t => t.AssigneeId)              
                .OnDelete(DeleteBehavior.SetNull);               

            // Ticket and Team Relationship
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Team)                            
                .WithMany(t => t.Tickets)                     
                .HasForeignKey(t => t.TeamAssignedId)          
                .OnDelete(DeleteBehavior.SetNull);              

            // Ticket and Attachment Relationship
            modelBuilder.Entity<Attachment>()
                .HasOne(a => a.Ticket)                          
                .WithMany(t => t.Attachments)                   
                .HasForeignKey(a => a.TicketId)                
                .OnDelete(DeleteBehavior.Cascade);               

            // Ticket and Feedback Relationship
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Ticket)                          
                .WithMany()                                     
                .HasForeignKey(f => f.TicketId)                
                .OnDelete(DeleteBehavior.Cascade);               

            // Team and User Relationship (Team Leader)
            modelBuilder.Entity<Team>()
                .HasOne(t => t.TeamLeader)
                .WithMany(u => u.TeamsLed) 
                .HasForeignKey(t => t.TeamLeaderId) 
                .OnDelete(DeleteBehavior.Restrict); 

            // Team and User Relationship (Members)
            modelBuilder.Entity<Team>()
                .HasMany(t => t.Users) 
                .WithMany(u => u.Teams) 
                .UsingEntity(j => j.ToTable("UserTeams"));               

            // TicketStatus and Ticket Relationship
            modelBuilder.Entity<TicketStatus>()
                .HasMany(s => s.Tickets)                        
                .WithOne()                                      
                .HasForeignKey(t => t.StatusId)                 
                .OnDelete(DeleteBehavior.Cascade);               

            // Announcement and User Relationship
            modelBuilder.Entity<Announcement>()
                .HasOne(a => a.Creator)                         
                .WithMany()                                     
                .HasForeignKey(a => a.CreatedBy)               
                .OnDelete(DeleteBehavior.Restrict);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
