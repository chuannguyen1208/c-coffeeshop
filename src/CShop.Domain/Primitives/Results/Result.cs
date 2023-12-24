namespace CShop.Domain.Primitives.Results;
public class Result : IResult
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

    public static readonly IResult Success = new Result(true, Error.None);
    public static IResult Failure(Error error) => new Result(false, error);
}
