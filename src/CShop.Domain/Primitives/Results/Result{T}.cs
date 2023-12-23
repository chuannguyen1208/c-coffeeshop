namespace CShop.Domain.Primitives.Results;

public class Result<T> : Result, IResult<T>
{
    private Result(bool isSuccess, Error error, T value) : base(isSuccess, error)
    {
        Value = value;
    }

    public T Value { get; }
    public static new Result<T> Success(T value) => new(true, Error.None, value);
    public static new Result<T> Failure(Error error) => new(isSuccess: false, error, value: default!);
}

