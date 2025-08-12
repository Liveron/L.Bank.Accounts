using System.Data;
using Npgsql;

namespace L.Bank.Accounts.Features.Accounts.AccrueAllInterests;

public interface IAccrueAllInterestsJob
{
    Task ExecuteAsync();
}

public sealed class AccrueAllInterestsJob(IConfiguration configuration) : IAccrueAllInterestsJob
{
    public async Task ExecuteAsync()
    {
        var connectionString = configuration.GetConnectionString("Postgres");
        await using var connection = new NpgsqlConnection(connectionString);
        await using var command = new NpgsqlCommand("accrue_all_interests", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }
}