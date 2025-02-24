namespace LibraryReservationManagementSystem.OperationResults;

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