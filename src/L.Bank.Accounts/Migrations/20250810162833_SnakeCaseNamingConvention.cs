using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace L.Bank.Accounts.Migrations
{
    /// <inheritdoc />
    public partial class SnakeCaseNamingConvention : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_accounts_AccountId",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transactions",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_accounts",
                table: "accounts");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "transactions",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Sum",
                table: "transactions",
                newName: "sum");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "transactions",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "transactions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "transactions",
                newName: "date_time");

            migrationBuilder.RenameColumn(
                name: "CounterpartyAccountId",
                table: "transactions",
                newName: "counterparty_account_id");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "transactions",
                newName: "account_id");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_DateTime",
                table: "transactions",
                newName: "ix_transactions_date_time");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_AccountId_DateTime",
                table: "transactions",
                newName: "ix_transactions_account_id_date_time");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "accounts",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "accounts",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "accounts",
                newName: "balance");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "accounts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "accounts",
                newName: "owner_id");

            migrationBuilder.RenameColumn(
                name: "OpenDate",
                table: "accounts",
                newName: "open_date");

            migrationBuilder.RenameColumn(
                name: "MaturityDate",
                table: "accounts",
                newName: "maturity_date");

            migrationBuilder.RenameColumn(
                name: "InterestRate",
                table: "accounts",
                newName: "interest_rate");

            migrationBuilder.RenameColumn(
                name: "CloseDate",
                table: "accounts",
                newName: "close_date");

            migrationBuilder.RenameIndex(
                name: "IX_accounts_OwnerId",
                table: "accounts",
                newName: "ix_accounts_owner_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_transactions",
                table: "transactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_accounts",
                table: "accounts",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_transactions_accounts_account_id",
                table: "transactions",
                column: "account_id",
                principalTable: "accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_transactions_accounts_account_id",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_transactions",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_accounts",
                table: "accounts");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "transactions",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "sum",
                table: "transactions",
                newName: "Sum");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "transactions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "transactions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "date_time",
                table: "transactions",
                newName: "DateTime");

            migrationBuilder.RenameColumn(
                name: "counterparty_account_id",
                table: "transactions",
                newName: "CounterpartyAccountId");

            migrationBuilder.RenameColumn(
                name: "account_id",
                table: "transactions",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "ix_transactions_date_time",
                table: "transactions",
                newName: "IX_transactions_DateTime");

            migrationBuilder.RenameIndex(
                name: "ix_transactions_account_id_date_time",
                table: "transactions",
                newName: "IX_transactions_AccountId_DateTime");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "accounts",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "currency",
                table: "accounts",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "balance",
                table: "accounts",
                newName: "Balance");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "accounts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "accounts",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "open_date",
                table: "accounts",
                newName: "OpenDate");

            migrationBuilder.RenameColumn(
                name: "maturity_date",
                table: "accounts",
                newName: "MaturityDate");

            migrationBuilder.RenameColumn(
                name: "interest_rate",
                table: "accounts",
                newName: "InterestRate");

            migrationBuilder.RenameColumn(
                name: "close_date",
                table: "accounts",
                newName: "CloseDate");

            migrationBuilder.RenameIndex(
                name: "ix_accounts_owner_id",
                table: "accounts",
                newName: "IX_accounts_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactions",
                table: "transactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_accounts",
                table: "accounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_accounts_AccountId",
                table: "transactions",
                column: "AccountId",
                principalTable: "accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
