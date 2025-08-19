using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace L.Bank.Accounts.Migrations
{
    /// <inheritdoc />
    public partial class InboxAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "inbox_consumed",
                columns: table => new
                {
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    handler = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inbox_consumed", x => x.message_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inbox_consumed");
        }
    }
}
