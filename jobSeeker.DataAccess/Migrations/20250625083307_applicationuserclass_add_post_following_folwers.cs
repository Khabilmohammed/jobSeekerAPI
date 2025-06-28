using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace jobSeeker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class applicationuserclass_add_post_following_folwers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Follows",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Follows",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Follows_ApplicationUserId",
                table: "Follows",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_ApplicationUserId1",
                table: "Follows",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AspNetUsers_ApplicationUserId",
                table: "Follows",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AspNetUsers_ApplicationUserId1",
                table: "Follows",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AspNetUsers_ApplicationUserId",
                table: "Follows");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AspNetUsers_ApplicationUserId1",
                table: "Follows");

            migrationBuilder.DropIndex(
                name: "IX_Follows_ApplicationUserId",
                table: "Follows");

            migrationBuilder.DropIndex(
                name: "IX_Follows_ApplicationUserId1",
                table: "Follows");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Follows");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Follows");
        }
    }
}
