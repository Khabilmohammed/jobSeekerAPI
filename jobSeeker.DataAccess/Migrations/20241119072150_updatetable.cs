using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace jobSeeker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId1",
                table: "JobPostings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_CompanyId1",
                table: "JobPostings",
                column: "CompanyId1");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_Companies_CompanyId1",
                table: "JobPostings",
                column: "CompanyId1",
                principalTable: "Companies",
                principalColumn: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_Companies_CompanyId1",
                table: "JobPostings");

            migrationBuilder.DropIndex(
                name: "IX_JobPostings_CompanyId1",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                table: "JobPostings");
        }
    }
}
