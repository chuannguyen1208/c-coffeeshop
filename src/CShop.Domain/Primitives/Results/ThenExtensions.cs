namespace CShop.Domain.Primitives.Results;
public static class ThenExtensions
{
    // sync to sync
    public static IResult<TOut> Then<TOut>(
        this IResult result,
        Func<TOut> action)
    {
        if (result.IsFailure)
        {
            return Result<TOut>.Failure(result.Error!);
        }

        var value = action();
        return Result<TOut>.Success(value);
    }

    // sync to async
    public static async Task<IResult<TOut>> Then<TOut>(
        this IResult result,
        Func<Task<TOut>> func)
    {
        if (result.IsFailure)
        {
            return Result<TOut>.Failure(result.Error!);
        }

        var value = await func();
        return Result<TOut>.Success(value);
    }

    // input: sync to sync
    public static IResult<TOut> Then<TIn, TOut>(
        this IResult<TIn> result,
        Func<TIn, TOut> func)
    {
        if (result.IsFailure)
        {
            return Result<TOut>.Failure(result.Error!);
        }

        var value = func(result.Value);
        return Result<TOut>.Success(value);
    }

    // input: sync to async
}
