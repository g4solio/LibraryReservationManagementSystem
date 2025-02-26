using LibraryReservationManagementSystem.DbContexts;
using LibraryReservationManagementSystem.Models;
using LibraryReservationManagementSystem.OperationResults;
using Microsoft.EntityFrameworkCore;

namespace LibraryReservationManagementSystem.Repositories;

public class ReservationRepository(NpgApplicationContext applicationContext) : IRepository<Reservation>, IAsyncDisposable
{
    public IOperationResult<Reservation> Add(Reservation entity)
    {
        var reservation = applicationContext.Reservations.Add(entity);

        if (reservation.State != EntityState.Added)
            return "Reservation could not be added".AsFailedOperation<Reservation>();

        return reservation.Entity.AsSuccessfulOperation();
    }

    public IOperationResult<Reservation> Update(Reservation entity)
    {
        var oldReservation = GetById(entity.Id);

        if (!oldReservation.IsSuccess)
            return oldReservation.Message.AsFailedOperation<Reservation>();

        oldReservation.Data!.Customer = entity.Customer;
        oldReservation.Data!.Book = entity.Book;
        oldReservation.Data!.ExpirationDate = entity.ExpirationDate;

        var reservation = applicationContext.Reservations.Update(oldReservation.Data!);

        if (reservation.State != EntityState.Modified)
            return "Reservation could not be updated".AsFailedOperation<Reservation>();

        return reservation.Entity.AsSuccessfulOperation();
    }

    public IOperationResult<Reservation> Delete(Reservation entity)
    {
        var reservation = applicationContext.Reservations.Remove(entity);

        if (reservation.State != EntityState.Deleted)
            return "Reservation could not be deleted".AsFailedOperation<Reservation>();

        return reservation.Entity.AsSuccessfulOperation();
    }

    public IOperationResult<Reservation> GetById(int id)
    {
        var reservation = applicationContext.Reservations.Find(id);

        if (reservation == null)
            return "Reservation could not be found".AsFailedOperation<Reservation>();

        return reservation!.AsSuccessfulOperation();
    }

    public IOperationResult<IList<Reservation>> GetAll()
    {
        var reservations = applicationContext.Reservations.ToList();

        if (reservations.Count == 0)
            return "No reservations found".AsFailedOperation<IList<Reservation>>();

        return reservations.AsSuccessfulOperation<IList<Reservation>>();
    }

    public IOperationResult<IList<Reservation>> GetByCondition(Func<Reservation, bool> condition)
    {
        var reservations = applicationContext.Reservations.Where(condition).ToList();

        if (reservations.Count == 0)
            return "No reservations found".AsFailedOperation<IList<Reservation>>();

        return reservations.AsSuccessfulOperation<IList<Reservation>>();
    }

    public void Dispose()
    {
        applicationContext.SaveChanges();
    }

    public async ValueTask DisposeAsync()
    {
        await applicationContext.SaveChangesAsync();
    }
}