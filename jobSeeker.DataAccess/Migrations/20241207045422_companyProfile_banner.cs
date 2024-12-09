using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace jobSeeker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class companyProfile_banner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePicture",
                table: "Companies",
                newName: "Logo");

            migrationBuilder.AddColumn<string>(
                name: "Banner",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Banner",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "Logo",
                table: "Companies",
                newName: "ProfilePicture");
        }
    }
}
