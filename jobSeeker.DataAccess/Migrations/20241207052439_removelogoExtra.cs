using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace jobSeeker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removelogoExtra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Companies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
