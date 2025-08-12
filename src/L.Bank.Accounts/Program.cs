using L.Bank.Accounts.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

builder.AddApplicationControllers();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseBackgroundJobs();

app.UseCors();

app.UseMbResultAuthorization();

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerOpenApi();
}

app.MapControllers();

app.Run();
