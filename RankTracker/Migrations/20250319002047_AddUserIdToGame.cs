using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RankTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Games",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
