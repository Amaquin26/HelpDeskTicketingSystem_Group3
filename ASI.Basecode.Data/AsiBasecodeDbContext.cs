using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ASI.Basecode.Data.Models;
using Microsoft.Extensions.Configuration;

namespace ASI.Basecode.Data
{
    public partial class AsiBasecodeDBContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AsiBasecodeDBContext()
        {
        }

        public AsiBasecodeDBContext(DbContextOptions<AsiBasecodeDBContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Load connection string from configuration if DI is not used
                var connectionString = _configuration?.GetConnectionString("DefaultConnection")
                                       ?? "Server=(localdb)\\MSSQLLocalDB;Database=HelpDeskTicketingDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketCategory> TicketCategories { get; set; }
        public virtual DbSet<TicketStatus> TicketStatuses { get; set; }
        public virtual DbSet<TicketPriority> TicketPriorities { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Super Admin" },
                new Role { RoleId = 2, RoleName = "Admin" },
                new Role { RoleId = 3, RoleName = "Agent" },
                new Role { RoleId = 4, RoleName = "User" }
            );

            modelBuilder.Entity<TicketStatus>().HasData(
                new TicketStatus { StatusId = 1, StatusName = "Open" },
                new TicketStatus { StatusId = 2, StatusName = "In Progress" },
                new TicketStatus { StatusId = 3, StatusName = "Resolved" },
                new TicketStatus { StatusId = 4, StatusName = "Closed" },
                new TicketStatus { StatusId = 5, StatusName = "On Hold" }
            );

            modelBuilder.Entity<TicketCategory>().HasData(
                new TicketCategory { CategoryId = 1, CategoryName = "Bug" },
                new TicketCategory { CategoryId = 2, CategoryName = "Feature Request" },
                new TicketCategory { CategoryId = 3, CategoryName = "Inquiry" },
                new TicketCategory { CategoryId = 4, CategoryName = "Support" },
                new TicketCategory { CategoryId = 5, CategoryName = "Maintenance" }
            );

            modelBuilder.Entity<TicketPriority>().HasData(
                new TicketPriority { PriorityId = 1, PriorityName = "Low" },
                new TicketPriority { PriorityId = 2, PriorityName = "Medium" },
                new TicketPriority { PriorityId = 3, PriorityName = "High" }
            );

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
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);


            });

            modelBuilder.Entity<User>().HasData(
                new User { 
                    UserId = "5c37344a-f1b4-47f3-a8e4-d2154591358e",
                    Name = "Agent 007",
                    Password = "Kw7+jFXwfGw/o6Mi2vJEXw==", //123
                    RoleId = 3,
                    Email = "resolveit.agent@mail.com",
                    CreatedBy = "System",
                    CreatedTime = DateTime.Now,
                    IsActive = true
                }
            );

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
            modelBuilder.Entity<User>()
                .HasOne(u => u.Team)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Team and User Relationship (Members)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Team) 
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // TicketStatus and Ticket Relationship
            modelBuilder.Entity<TicketStatus>()
                .HasMany(s => s.Tickets)                        
                .WithOne(t => t.Status)                                      
                .HasForeignKey(t => t.StatusId)                 
                .OnDelete(DeleteBehavior.Cascade);

            // TicketCategory and Ticket Relationship
            modelBuilder.Entity<TicketCategory>()
                .HasMany(s => s.Tickets)
                .WithOne(t => t.Category)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // TicketPriority and Ticket Relationship
            modelBuilder.Entity<TicketPriority>()
                .HasMany(s => s.Tickets)
                .WithOne(t => t.Priority)
                .HasForeignKey(t => t.PriorityId)
                .OnDelete(DeleteBehavior.Cascade);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
