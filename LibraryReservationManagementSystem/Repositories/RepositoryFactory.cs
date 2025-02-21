using LibraryReservationManagementSystem.DbContexts;
using LibraryReservationManagementSystem.Models;

namespace LibraryReservationManagementSystem.Repositories;

public class RepositoryFactory(ServiceProvider serviceProvider)
{
    private readonly ServiceProvider _serviceProvider = serviceProvider;

    public IRepository<T> GetRepository<T>() where T : class
    {
        return typeof(T) switch
        {
            { } book when book == typeof(Book) => _serviceProvider.GetService<BookRepository>() as IRepository<T>,
            { } customer when customer == typeof(Customer) => _serviceProvider.GetService<CustomerRepository>() as IRepository<T>,
            { } reservation when reservation == typeof(Reservation) => _serviceProvider.GetService<ReservationRepository>() as IRepository<T>,
            _ => null
        } ?? throw new ArgumentException($"Invalid type {typeof(T)}");
    }

}








