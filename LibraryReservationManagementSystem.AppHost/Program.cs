var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.LibraryReservationManagementSystem>("libraryreservationmanagementsystem");

builder.Build().Run();
