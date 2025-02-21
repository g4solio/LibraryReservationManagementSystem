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


public interface IOperationResult
{
    public bool IsSuccess { get; }
    public string Message { get; }
}

public interface IOperationResult<T> : IOperationResult where T : class?
{
    public T? Data { get; }

}

public static class OperationResultExtension
{
    public static IOperationResult<T> AsSuccessfulOperation<T>(this T entity) where T : class
    {
        return new OperationResult<T>()
        {
            Data = entity,
            IsSuccess = true,
            Message = null
        };
    }

    public static IOperationResult<T> AsFailedOperation<T>(this Exception message) where T : class
    {
        return new OperationResult<T>()
        {
            Data = null,
            IsSuccess = false,
            Message = message.Message
        };
    }

    public static IOperationResult<T> AsFailedOperation<T>(this string message) where T : class
    {
        return new OperationResult<T>()
        {
            Data = null,
            IsSuccess = false,
            Message = message
        };
    }

    private class OperationResult<T> : IOperationResult<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
