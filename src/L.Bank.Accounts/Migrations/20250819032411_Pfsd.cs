using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace L.Bank.Accounts.Migrations
{
    /// <inheritdoc />
    public partial class Pfsd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "inbox_dead_letters");

            migrationBuilder.CreateTable(
                name: "InboxDeadEventEntries",
                schema: "inbox_dead_letters",
                columns: table => new
                {
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    received_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    handler = table.Column<string>(type: "text", nullable: false),
                    payload = table.Column<string>(type: "text", nullable: false),
                    error = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inbox_dead_event_entries", x => x.message_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InboxDeadEventEntries",
                schema: "inbox_dead_letters");
        }
    }
}
