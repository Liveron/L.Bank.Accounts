using L.Bank.Accounts.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

builder.AddApplicationControllers();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (!app.Environment.IsTesting())
    app.UseBackgroundJobs();

app.UseCors();

app.UseMbResultAuthorization();

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerOpenApi();
    app.MapHealthChecks();
}

app.MapControllers();

app.Run();
