using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RankTracker.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUserUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "RankEntries",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RankEntries_ApplicationUserId",
                table: "RankEntries",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RankEntries_AspNetUsers_ApplicationUserId",
                table: "RankEntries",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RankEntries_AspNetUsers_ApplicationUserId",
                table: "RankEntries");

            migrationBuilder.DropIndex(
                name: "IX_RankEntries_ApplicationUserId",
                table: "RankEntries");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "RankEntries");
        }
    }
}
