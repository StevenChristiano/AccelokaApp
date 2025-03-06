using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StevenAccelokaAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookedTickets_BookingId",
                table: "BookedTickets",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookedTickets_Bookings_BookingId",
                table: "BookedTickets",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookedTickets_Bookings_BookingId",
                table: "BookedTickets");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_BookedTickets_BookingId",
                table: "BookedTickets");
        }
    }
}
