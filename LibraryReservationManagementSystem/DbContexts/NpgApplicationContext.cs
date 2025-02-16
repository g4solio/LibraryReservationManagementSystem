using LibraryReservationManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryReservationManagementSystem.DbContexts;

public class NpgApplicationContext(DbContextOptions<NpgApplicationContext> options) : DbContext(options)
{
    public DbSet<Models.Book> Books { get; set; }

    public DbSet<Models.Customer> Customers { get; set; }

    public DbSet<Models.Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().HasIndex(b => b.ISBN).IsUnique();
        modelBuilder.Entity<Customer>().HasIndex(c => c.Email).IsUnique();


        modelBuilder.Entity<Models.Book>().ToTable("Books");
        modelBuilder.Entity<Models.Customer>().ToTable("Customers");
        modelBuilder.Entity<Models.Reservation>().ToTable("Reservations");
    }
}