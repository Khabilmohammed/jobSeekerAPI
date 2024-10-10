using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace jobSeeker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatestorysettting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Stories");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationTime",
                table: "Stories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationTime",
                table: "Stories");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Stories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
