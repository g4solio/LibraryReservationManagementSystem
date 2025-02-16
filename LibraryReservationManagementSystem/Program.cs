using LibraryReservationManagementSystem.DbContexts;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.AddNpgsqlDbContext<NpgApplicationContext>(connectionName: "LRMS");

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(_ => _.Servers = []); //TODO(DG): Normalize code after deployed fix, https://github.com/dotnet/aspnetcore/issues/57332
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
