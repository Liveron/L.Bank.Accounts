using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace L.Bank.Accounts.Migrations
{
    /// <inheritdoc />
    public partial class PendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "occured_at",
                table: "event_entries",
                newName: "saved_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "saved_at",
                table: "event_entries",
                newName: "occured_at");
        }
    }
}
