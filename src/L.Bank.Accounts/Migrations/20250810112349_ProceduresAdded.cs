using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace L.Bank.Accounts.Migrations
{
    /// <inheritdoc />
    public partial class ProceduresAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE PROCEDURE accrue_interest(IN account_id UUID)
                LANGUAGE plpgsql
                AS $$
                BEGIN
                    UPDATE accounts
                    SET balance = balance * (interest_rate / 100) * ((CURRENT_DATE - open_date) / 365), 
                        close_date = CURRENT_DATE
                    WHERE id = account_id;
                END;
                $$
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS accrue_interest(UUID)");
        }
    }
}
