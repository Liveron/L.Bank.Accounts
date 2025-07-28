using L.Bank.Accounts.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

builder.AddAppControllers();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
