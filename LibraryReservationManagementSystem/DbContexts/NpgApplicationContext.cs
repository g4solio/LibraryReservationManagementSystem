using LibraryReservationManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryReservationManagementSystem.DbContexts;

public class NpgApplicationContext(DbContextOptions<NpgApplicationContext> options) : DbContext(options)
{

    public DbSet<Models.Book> Books => Set<Book>();

    public DbSet<Models.Customer> Customers => Set<Customer>();

    public DbSet<Models.Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().HasIndex(b => b.ISBN).IsUnique();
        modelBuilder.Entity<Customer>().HasIndex(c => c.Email).IsUnique();

        modelBuilder.Entity<Reservation>().Navigation(r => r.Customer).AutoInclude();
        modelBuilder.Entity<Reservation>().Navigation(r => r.Book).AutoInclude();


        modelBuilder.Entity<Models.Book>().ToTable("Books");
        modelBuilder.Entity<Models.Customer>().ToTable("Customers");
        modelBuilder.Entity<Models.Reservation>().ToTable("Reservations");
    }
}