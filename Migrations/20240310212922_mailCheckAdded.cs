using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.Migrations
{
    /// <inheritdoc />
    public partial class mailCheckAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isMailSendSuccess",
                table: "TeamApplicationForms",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isMailSendSuccess",
                table: "TeamApplicationForms");
        }
    }
}
