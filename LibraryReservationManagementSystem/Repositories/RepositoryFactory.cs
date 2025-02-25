using LibraryReservationManagementSystem.Models;

namespace LibraryReservationManagementSystem.Repositories;

public class RepositoryFactory(IServiceProvider serviceProvider) : IRepositoryFactory
{

    public IRepository<T> GetRepository<T>() where T : class
    {
        return typeof(T) switch
        {
            { } book when book == typeof(Book) => serviceProvider.GetService<BookRepository>() as IRepository<T>,
            { } customer when customer == typeof(Customer) => serviceProvider.GetService<CustomerRepository>() as IRepository<T>,
            { } reservation when reservation == typeof(Reservation) => serviceProvider.GetService<ReservationRepository>() as IRepository<T>,
            _ => null
        } ?? throw new ArgumentException($"Invalid type {typeof(T)}");
    }

}

public interface IRepositoryFactory
{
    IRepository<T> GetRepository<T>() where T : class;
}








