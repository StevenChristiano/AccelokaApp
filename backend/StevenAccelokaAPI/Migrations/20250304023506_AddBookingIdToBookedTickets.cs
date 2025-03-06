using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StevenAccelokaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingIdToBookedTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "BookedTickets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "BookedTickets");
        }
    }
}
