using Microsoft.EntityFrameworkCore;

namespace L.Bank.Accounts.Extensions;

internal static class MigrationExtensions
{
    public static void AddMigrations<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddHostedService<MigrationsBackgroundService<TContext>>();
    }

    private class MigrationsBackgroundService<TContext>(IServiceProvider serviceProvider)
        : BackgroundService where TContext : DbContext
    {
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            await context.Database.MigrateAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken _)
        {
            return Task.CompletedTask;
        }
    }
}