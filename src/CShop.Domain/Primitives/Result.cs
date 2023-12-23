namespace CShop.Domain.Primitives;

public sealed record Error(string Code, string? Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static implicit operator Result(Error error) => Result.Failure(error);
}

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
}

public class Result<T> : Result
{
    private Result(bool isSuccess, Error error, T value) : base(isSuccess, error)
    {
        Value = value;
    }

    public T Value { get; }
    public static Result<T> Success(T value) => new(true, Error.None, value);
}

public static class ResultExtensions
{
    public static async Task<Result<TOut>> Then<TOut>(
        this Result _, 
        Func<Task<TOut>> func)
    {
        var res = await func();
        return Result<TOut>.Success(res);
    }

    #region Tap
    public static async Task<Result<TIn>> Tap<TIn>(
        this Task<Result<TIn>> result, 
        Func<Task> func)
    {
        await func();
        return await result;
    }
    #endregion
}