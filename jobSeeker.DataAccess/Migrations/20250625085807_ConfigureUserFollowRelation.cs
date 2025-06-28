using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace jobSeeker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureUserFollowRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "FollowingId",
                table: "Follows",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<string>(
                name: "FollowerId",
                table: "Follows",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("Relational:ColumnOrder", 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FollowingId",
                table: "Follows",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<string>(
                name: "FollowerId",
                table: "Follows",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .OldAnnotation("Relational:ColumnOrder", 0);

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
    }
}
