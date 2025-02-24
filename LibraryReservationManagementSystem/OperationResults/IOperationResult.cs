namespace LibraryReservationManagementSystem.OperationResults;

public interface IOperationResult<T> : IOperationResult where T : class?
{
    public T? Data { get; }

}

public interface IOperationResult
{
    public bool IsSuccess { get; }
    public string Message { get; }
}