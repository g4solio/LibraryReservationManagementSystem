using LibraryReservationManagementSystem.Models;
using LibraryReservationManagementSystem.OperationResults;
using LibraryReservationManagementSystem.Repositories;

namespace LibraryReservationManagementSystem.Services;

public class LibraryReservationManagementService(IRepositoryFactory repositoryFactory, ILogger<LibraryReservationManagementService> logger) : ILibraryReservationManagementService
{
    public IOperationResult<Reservation> RentABook(int customerId, int bookId)
    {
        using var customerRepository = repositoryFactory.GetRepository<Customer>();
        using var bookRepository = repositoryFactory.GetRepository<Book>();

        var customer = customerRepository.GetById(customerId);

        if (!customer.IsSuccess)
            return customer.Message.AsFailedOperation<Reservation>();

        var book = bookRepository.GetById(bookId);

        if (!book.IsSuccess)
            return book.Message.AsFailedOperation<Reservation>();

        if (book.Data!.Status != Book.StatusEnum.Available)
            return "Book is not available".AsFailedOperation<Reservation>();

        book.Data.Status = Book.StatusEnum.Unavailable;
        //bookRepository.Update(book.Data);

        var reservation = new Reservation
        {
            Customer = customer.Data!,
            Book = book.Data!,
            ExpirationDate = DateTime.Now.AddDays(7),
            ReservationDate = DateTime.Now
        };

        using var reservationRepository = repositoryFactory.GetRepository<Reservation>();
        var result = reservationRepository.Add(reservation);

        return result;
    }

    public IOperationResult<Reservation> ExtendReservation(int reservationId, DateTime newExpirationDate)
    {
        using var reservationRepository = repositoryFactory.GetRepository<Reservation>();

        var reservation = reservationRepository.GetById(reservationId);
        if (!reservation.IsSuccess)
            return reservation.Message.AsFailedOperation<Reservation>();

        reservation.Data!.ExpirationDate = newExpirationDate;

        //var result = reservationRepository.Update(reservation.Data!);

        return reservation;
    }

    public IOperationResult<IEnumerable<Reservation>> GetReservationsByCustomer(int customerId)
    {
        using var reservationRepository = repositoryFactory.GetRepository<Reservation>();
        using var customerRepository = repositoryFactory.GetRepository<Customer>();

        var customer = customerRepository.GetById(customerId);

        if (!customer.IsSuccess)
            return customer.Message.AsFailedOperation<IEnumerable<Reservation>>();

        var reservations = reservationRepository.GetByCondition(r => r.Customer.Id == customer.Data!.Id);

        return reservations.IsSuccess
            ? reservations.Data!.AsSuccessfulOperation<IEnumerable<Reservation>>()
            : reservations.Message.AsFailedOperation<IEnumerable<Reservation>>();
    }
}