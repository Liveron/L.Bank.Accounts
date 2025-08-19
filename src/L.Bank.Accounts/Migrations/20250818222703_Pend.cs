using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace L.Bank.Accounts.Migrations
{
    /// <inheritdoc />
    public partial class Pend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "handler",
                table: "inbox_consumed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "handler",
                table: "inbox_consumed",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
