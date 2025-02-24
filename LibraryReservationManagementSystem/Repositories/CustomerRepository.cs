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
        applicationContext.SaveChanges();
        
        if (customer.State != EntityState.Added)
            "Customer could not be added".AsFailedOperation<Customer>();

        return customer.Entity.AsSuccessfulOperation();
    }

    public IOperationResult<Customer> Update(Customer entity)
    {
        var customer = applicationContext.Customers.Update(entity);
        applicationContext.SaveChanges();

        if (customer.State != EntityState.Modified)
            "Customer could not be updated".AsFailedOperation<Customer>();
        
        return customer.Entity.AsSuccessfulOperation();
    }

    public IOperationResult<Customer> Delete(Customer entity)
    {
        var customer = applicationContext.Customers.Remove(entity);
        applicationContext.SaveChanges();

        if (customer.State != EntityState.Deleted)
            "Customer could not be deleted".AsFailedOperation<Customer>();

        return customer.Entity.AsSuccessfulOperation();
    }

    public IOperationResult<Customer> GetById(int id)
    {
        var customer = applicationContext.Customers.Find(id);
        
        if (customer == null)
            "Customer could not be found".AsFailedOperation<Customer>();
        
        return customer!.AsSuccessfulOperation();
    }

    public IOperationResult<IList<Customer>> GetAll()
    {
        var customers = applicationContext.Customers.ToList();
        
        if (customers.Count == 0)
            "No customers found".AsFailedOperation<IEnumerable<Customer>>();
        
        return customers.AsSuccessfulOperation<IList<Customer>>();
    }

    public IOperationResult<IList<Customer>> GetByCondition(Func<Customer, bool> condition)
    {
        var customers = applicationContext.Customers.Where(condition).ToList();

        if (customers.Count == 0)
            "No customers found".AsFailedOperation<IEnumerable<Customer>>();

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