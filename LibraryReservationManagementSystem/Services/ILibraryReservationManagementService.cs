using LibraryReservationManagementSystem.Models;
using LibraryReservationManagementSystem.OperationResults;

namespace LibraryReservationManagementSystem.Services;

public interface ILibraryReservationManagementService
{
    IOperationResult<Reservation> RentABook(int customerId, int bookId);

    IOperationResult<Reservation> ExtendReservation(int reservationId, DateTime newExpirationDate);

    IOperationResult<IEnumerable<Reservation>> GetReservationsByCustomer(int customerId);
}