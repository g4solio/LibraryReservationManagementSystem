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
        applicationContext.SaveChanges();

        if (reservation.State != EntityState.Added)
            "Reservation could not be added".AsFailedOperation<Reservation>();
        
        return reservation.Entity.AsSuccessfulOperation();
    }

    public IOperationResult<Reservation> Update(Reservation entity)
    {
        var reservation = applicationContext.Reservations.Update(entity);
        applicationContext.SaveChanges();
        
        if (reservation.State != EntityState.Modified)
            "Reservation could not be updated".AsFailedOperation<Reservation>();

        return reservation.Entity.AsSuccessfulOperation();
    }

    public IOperationResult<Reservation> Delete(Reservation entity)
    {
        var reservation = applicationContext.Reservations.Remove(entity);
        applicationContext.SaveChanges();

        if (reservation.State != EntityState.Deleted)
            "Reservation could not be deleted".AsFailedOperation<Reservation>();
        
        return reservation.Entity.AsSuccessfulOperation();
    }

    public IOperationResult<Reservation> GetById(int id)
    {
        var reservation = applicationContext.Reservations.Find(id);
        
        if (reservation == null)
            "Reservation could not be found".AsFailedOperation<Reservation>();
        
        return reservation!.AsSuccessfulOperation();
    }

    public IOperationResult<IList<Reservation>> GetAll()
    {
        var reservations = applicationContext.Reservations.ToList();

        if (reservations.Count == 0)
            "No reservations found".AsFailedOperation<IList<Reservation>>();

        return reservations.AsSuccessfulOperation<IList<Reservation>>();
    }

    public IOperationResult<IList<Reservation>> GetByCondition(Func<Reservation, bool> condition)
    {
        var reservations = applicationContext.Reservations.Where(condition).ToList();

        if (reservations.Count == 0)
            "No reservations found".AsFailedOperation<IList<Reservation>>();

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