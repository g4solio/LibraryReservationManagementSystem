using LibraryReservationManagementSystem.DbContexts;
using LibraryReservationManagementSystem.Models;
using LibraryReservationManagementSystem.OperationResults;
using Microsoft.EntityFrameworkCore;

namespace LibraryReservationManagementSystem.Repositories;

public class CustomerRepository(NpgApplicationContext applicationContext) : IRepository<Customer>, IAsyncDisposable
{
    public IOperationResult<Customer> Add(Customer entity)
    {
        var customer = applicationContext.Customers.Add(entity);

        if (customer.State != EntityState.Added)
            return "Customer could not be added".AsFailedOperation<Customer>();

        return customer.Entity.AsSuccessfulOperation();
    }

    public IOperationResult<Customer> Update(Customer entity)
    {
        var oldValue = GetById(entity.Id);

        if (!oldValue.IsSuccess)
            return oldValue.Message.AsFailedOperation<Customer>();

        oldValue.Data!.FirstName = entity.FirstName;
        oldValue.Data!.LastName = entity.LastName;
        oldValue.Data!.Email = entity.Email;

        var customer = applicationContext.Customers.Update(oldValue.Data!);

        if (customer.State != EntityState.Modified)
            return "Customer could not be updated".AsFailedOperation<Customer>();

        return customer.Entity.AsSuccessfulOperation();
    }

    public IOperationResult<Customer> Delete(Customer entity)
    {
        var customer = applicationContext.Customers.Remove(entity);

        if (customer.State != EntityState.Deleted)
            return "Customer could not be deleted".AsFailedOperation<Customer>();

        return customer.Entity.AsSuccessfulOperation();
    }

    public IOperationResult<Customer> GetById(int id)
    {
        var customer = applicationContext.Customers.Find(id);

        if (customer == null)
            return "Customer could not be found".AsFailedOperation<Customer>();

        return customer!.AsSuccessfulOperation();
    }

    public IOperationResult<IList<Customer>> GetAll()
    {
        var customers = applicationContext.Customers.ToList();

        if (customers.Count == 0)
            return "No customers found".AsFailedOperation<IList<Customer>>();

        return customers.AsSuccessfulOperation<IList<Customer>>();
    }

    public IOperationResult<IList<Customer>> GetByCondition(Func<Customer, bool> condition)
    {
        var customers = applicationContext.Customers.Where(condition).ToList();

        if (customers.Count == 0)
            return "No customers found".AsFailedOperation<IList<Customer>>();

        return customers.AsSuccessfulOperation<IList<Customer>>();
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