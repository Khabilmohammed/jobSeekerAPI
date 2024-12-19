using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace jobSeeker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class sharetablechange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shares_AspNetUsers_UserId",
                table: "Shares");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Shares",
                newName: "SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Shares_UserId",
                table: "Shares",
                newName: "IX_Shares_SenderId");

            migrationBuilder.AddColumn<string>(
                name: "RecipientId",
                table: "Shares",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Shares_RecipientId",
                table: "Shares",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_AspNetUsers_RecipientId",
                table: "Shares",
                column: "RecipientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_AspNetUsers_SenderId",
                table: "Shares",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shares_AspNetUsers_RecipientId",
                table: "Shares");

            migrationBuilder.DropForeignKey(
                name: "FK_Shares_AspNetUsers_SenderId",
                table: "Shares");

            migrationBuilder.DropIndex(
                name: "IX_Shares_RecipientId",
                table: "Shares");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "Shares");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Shares",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Shares_SenderId",
                table: "Shares",
                newName: "IX_Shares_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_AspNetUsers_UserId",
                table: "Shares",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
