using LibraryReservationManagementSystem.DbContexts;
using LibraryReservationManagementSystem.Repositories;
using LibraryReservationManagementSystem.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<NpgApplicationContext>(connectionName: "LRMS");

// Add services to the container.
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<ReservationRepository>();

builder.Services.AddScoped<ILibraryReservationManagementService, LibraryReservationManagementService>();

builder.Services.AddTransient<IRepositoryFactory, RepositoryFactory>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

//Applying migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NpgApplicationContext>();
    db.Database.EnsureCreated();
}


app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(_ => _.Servers = []); //TODO(DG): Normalize code after deployed fix, https://github.com/dotnet/aspnetcore/issues/57332
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
