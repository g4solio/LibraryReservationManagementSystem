var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);

var postgresDb = builder.AddPostgres("postgres", username, password).WithPgWeb()
    .AddDatabase("LRMS");

builder.AddProject<Projects.LibraryReservationManagementSystem>("libraryreservationmanagementsystem")
    .WithReference(postgresDb)
    .WaitFor(postgresDb);

builder.Build().Run();
