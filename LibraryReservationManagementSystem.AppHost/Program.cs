var builder = DistributedApplication.CreateBuilder(args);

var postgresDb = builder.AddPostgres("postgres").WithPgWeb().AddDatabase("LRMS");

builder.AddProject<Projects.LibraryReservationManagementSystem>("libraryreservationmanagementsystem")
    .WithReference(postgresDb);

builder.Build().Run();
