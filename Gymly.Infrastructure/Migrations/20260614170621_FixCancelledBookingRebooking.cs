using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCancelledBookingRebooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_SessionId_MemberId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SessionId_MemberId",
                table: "Bookings",
                columns: new[] { "SessionId", "MemberId" },
                unique: true,
                filter: "[IsCancelled] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_SessionId_MemberId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SessionId_MemberId",
                table: "Bookings",
                columns: new[] { "SessionId", "MemberId" },
                unique: true);
        }
    }
}
