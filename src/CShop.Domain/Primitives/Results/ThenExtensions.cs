namespace CShop.Domain.Primitives.Results;
public static class ThenExtensions
{
    public static async Task<IResult<T>> Then<T>(
        this IResult result,
        Func<Task<T>> func)
    {
        if (result.IsFailure)
        {
            return Result<T>.Failure(result.Error!);
        }

        var value = await func();
        return Result<T>.Success(value);
    }
}
