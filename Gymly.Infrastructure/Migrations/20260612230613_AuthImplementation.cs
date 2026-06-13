using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuthImplementation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM Classes WHERE Id NOT IN (SELECT MIN(Id) FROM Classes GROUP BY Name)");

            migrationBuilder.Sql(@"DELETE FROM Trainers WHERE Id NOT IN (SELECT MIN(Id) FROM Trainers GROUP BY Email)");

            migrationBuilder.Sql(@"DELETE FROM Plans WHERE Id NOT IN (SELECT MIN(Id) FROM Plans GROUP BY Title)");

            migrationBuilder.Sql(@"DELETE FROM Members WHERE Id NOT IN (SELECT MIN(Id) FROM Members GROUP BY Email)");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_TrainerId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Memberships_MemberId",
                table: "Memberships");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceLogs_MemberId",
                table: "AttendanceLogs");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_Email",
                table: "Trainers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_StartTime",
                table: "Sessions",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_TrainerId_StartTime_EndTime",
                table: "Sessions",
                columns: new[] { "TrainerId", "StartTime", "EndTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Plans_Title",
                table: "Plans",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_MemberId_Status_EndDate",
                table: "Memberships",
                columns: new[] { "MemberId", "Status", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_Name",
                table: "Classes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SessionId_IsCancelled",
                table: "Bookings",
                columns: new[] { "SessionId", "IsCancelled" });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceLogs_MemberId_ScannedAt",
                table: "AttendanceLogs",
                columns: new[] { "MemberId", "ScannedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trainers_Email",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_StartTime",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_TrainerId_StartTime_EndTime",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Plans_Title",
                table: "Plans");

            migrationBuilder.DropIndex(
                name: "IX_Memberships_MemberId_Status_EndDate",
                table: "Memberships");

            migrationBuilder.DropIndex(
                name: "IX_Classes_Name",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_SessionId_IsCancelled",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceLogs_MemberId_ScannedAt",
                table: "AttendanceLogs");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_TrainerId",
                table: "Sessions",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_MemberId",
                table: "Memberships",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceLogs_MemberId",
                table: "AttendanceLogs",
                column: "MemberId");
        }
    }
}
