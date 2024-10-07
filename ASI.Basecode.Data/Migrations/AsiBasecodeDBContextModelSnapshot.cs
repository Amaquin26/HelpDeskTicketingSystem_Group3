﻿// <auto-generated />
using System;
using ASI.Basecode.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ASI.Basecode.Data.Migrations
{
    [DbContext(typeof(AsiBasecodeDBContext))]
    partial class AsiBasecodeDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ASI.Basecode.Data.Models.Announcement", b =>
                {
                    b.Property<int>("AnnouncementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AnnouncementId"), 1L, 1);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("AnnouncementId");

                    b.HasIndex("CreatedBy");

                    b.ToTable("Announcements");
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TicketId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.Feedback", b =>
                {
                    b.Property<int>("FeedbackId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FeedbackId"), 1L, 1);

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<int>("TicketId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(50)");

                    b.HasKey("FeedbackId");

                    b.HasIndex("TicketId");

                    b.HasIndex("UserId");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"), 1L, 1);

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TeamId"), 1L, 1);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("TeamLeaderId")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("TeamName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeamSpecialization")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("TeamId");

                    b.HasIndex("TeamLeaderId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.Ticket", b =>
                {
                    b.Property<int>("TicketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TicketId"), 1L, 1);

                    b.Property<string>("AssigneeId")
                        .HasColumnType("varchar(50)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ResolvedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<int?>("StatusId1")
                        .HasColumnType("int");

                    b.Property<int?>("TeamAssignedId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("TicketId");

                    b.HasIndex("AssigneeId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("StatusId");

                    b.HasIndex("StatusId1");

                    b.HasIndex("TeamAssignedId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.TicketCategory", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"), 1L, 1);

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.ToTable("TicketCategories");
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.TicketStatus", b =>
                {
                    b.Property<int>("StatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StatusId"), 1L, 1);

                    b.Property<string>("StatusName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StatusId");

                    b.ToTable("TicketStatuses");
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.HasIndex(new[] { "UserId" }, "UQ__Users__1788CC4D5F4A160F")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TeamUser", b =>
                {
                    b.Property<int>("TeamsTeamId")
                        .HasColumnType("int");

                    b.Property<string>("UsersUserId")
                        .HasColumnType("varchar(50)");

                    b.HasKey("TeamsTeamId", "UsersUserId");

                    b.HasIndex("UsersUserId");

                    b.ToTable("UserTeams", (string)null);
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.Announcement", b =>
                {
                    b.HasOne("ASI.Basecode.Data.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.Attachment", b =>
                {
                    b.HasOne("ASI.Basecode.Data.Models.Ticket", "Ticket")
                        .WithMany("Attachments")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.Feedback", b =>
                {
                    b.HasOne("ASI.Basecode.Data.Models.Ticket", "Ticket")
                        .WithMany()
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASI.Basecode.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Ticket");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.Team", b =>
                {
                    b.HasOne("ASI.Basecode.Data.Models.User", "TeamLeader")
                        .WithMany("TeamsLed")
                        .HasForeignKey("TeamLeaderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("TeamLeader");
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.Ticket", b =>
                {
                    b.HasOne("ASI.Basecode.Data.Models.User", "Assignee")
                        .WithMany("Tickets")
                        .HasForeignKey("AssigneeId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("ASI.Basecode.Data.Models.TicketCategory", "Category")
                        .WithMany("Tickets")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASI.Basecode.Data.Models.TicketStatus", null)
                        .WithMany("Tickets")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASI.Basecode.Data.Models.TicketStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId1");

                    b.HasOne("ASI.Basecode.Data.Models.Team", "Team")
                        .WithMany("Tickets")
                        .HasForeignKey("TeamAssignedId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Assignee");

                    b.Navigation("Category");

                    b.Navigation("Status");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.User", b =>
                {
                    b.HasOne("ASI.Basecode.Data.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("TeamUser", b =>
                {
                    b.HasOne("ASI.Basecode.Data.Models.Team", null)
                        .WithMany()
                        .HasForeignKey("TeamsTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASI.Basecode.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.Team", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.Ticket", b =>
                {
                    b.Navigation("Attachments");
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.TicketCategory", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.TicketStatus", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("ASI.Basecode.Data.Models.User", b =>
                {
                    b.Navigation("TeamsLed");

                    b.Navigation("Tickets");
                });
#pragma warning restore 612, 618
        }
    }
}
