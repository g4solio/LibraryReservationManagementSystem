using LibraryReservationManagementSystem.DbContexts;
using LibraryReservationManagementSystem.Models;
using LibraryReservationManagementSystem.Repositories;
using LibraryReservationManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<NpgApplicationContext>(connectionName: "LRMS");

// Add services to the container.
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<BookRepository>();

builder.Services.AddScoped<ILibraryReservationManagementService, LibraryReservationManagementService>();

builder.Services.AddSingleton<IRepositoryFactory, RepositoryFactory>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

//Applying migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NpgApplicationContext>();
    db.Database.EnsureCreated();
}


app.MapDefaultEndpoints();

app.MapHealthChecks("/health");

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
