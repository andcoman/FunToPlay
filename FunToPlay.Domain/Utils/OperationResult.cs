namespace FunToPlay.Domain.Utils
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public T Value { get; }

        private OperationResult(T value, bool isSuccess, string error)
        {
            Value = value;
            IsSuccess = isSuccess;
            Error = error;
        }

        public static OperationResult<T?> Success(T value) => new OperationResult<T?>(value, true, null);

        public static OperationResult<T?> Failure(string error) => new OperationResult<T?>(default, false, error);
    }

    public static class OperationResult
    {
        public static OperationResult<T?> Success<T>(T value) => OperationResult<T>.Success(value);

        public static OperationResult<T?> Failure<T>(string error) => OperationResult<T>.Failure(error);
    }
}