namespace CShop.Domain.Primitives.Results;
public interface IResult
{
    bool IsSuccess { get; }
    bool IsFailure { get; }
    Error? Error { get; }
}

public interface IResult<T> : IResult
{
    T Value { get; }
}
