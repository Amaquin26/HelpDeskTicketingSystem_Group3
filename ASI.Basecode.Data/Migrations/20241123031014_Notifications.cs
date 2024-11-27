using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASI.Basecode.Data.Migrations
{
    public partial class Notifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Users_TeamLeaderId",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "UserTeams");

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "TicketCategories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CategoryName",
                value: "Feature Request");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "5c37344a-f1b4-47f3-a8e4-d2154591358e",
                column: "CreatedTime",
                value: new DateTime(2024, 11, 23, 11, 10, 8, 303, DateTimeKind.Local).AddTicks(7619));

            migrationBuilder.CreateIndex(
                name: "IX_Users_TeamId",
                table: "Users",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Users_TeamLeaderId",
                table: "Teams",
                column: "TeamLeaderId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Teams_TeamId",
                table: "Users",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Users_TeamLeaderId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Teams_TeamId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Users_TeamId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserTeams",
                columns: table => new
                {
                    TeamsTeamId = table.Column<int>(type: "int", nullable: false),
                    UsersUserId = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTeams", x => new { x.TeamsTeamId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_UserTeams_Teams_TeamsTeamId",
                        column: x => x.TeamsTeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTeams_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "TicketCategories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CategoryName",
                value: "Feature Request'");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "5c37344a-f1b4-47f3-a8e4-d2154591358e",
                column: "CreatedTime",
                value: new DateTime(2024, 10, 12, 12, 3, 43, 954, DateTimeKind.Local).AddTicks(9197));

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_UsersUserId",
                table: "UserTeams",
                column: "UsersUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Users_TeamLeaderId",
                table: "Teams",
                column: "TeamLeaderId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
