using LibraryReservationManagementSystem.OperationResults;

namespace LibraryReservationManagementSystem.Repositories;

public interface IRepository<T> : IDisposable where T : class
{
    IOperationResult<T> Add(T entity);

    IOperationResult<T> Update(T entity);

    IOperationResult<T> Delete(T entity);

    IOperationResult<T> GetById(int id);

    IOperationResult<IList<T>> GetAll();

    IOperationResult<IList<T>> GetByCondition(Func<T, bool> condition);
}