namespace CShop.Domain.Primitives.Results;
public static class TapExtensions
{
    // sync_sync
    public static IResult<T> Tap<T>(
        this IResult<T> result,
        Action<T> action)
    {
        if (result.IsFailure)
        {
            return Result<T>.Failure(result.Error!);
        }

        action(result.Value);
        return result;
    }

    // sync_async
    public static async Task<IResult<T>> Tap<T>(
        this IResult<T> result,
        Func<T, Task> func)
    {
        if (result.IsFailure)
        {
            return Result<T>.Failure(result.Error!);
        }

        await func(result.Value);
        return result;
    }

    // async_sync => sync_sync
    public static async Task<IResult<T>> Tap<T>(
        this Task<IResult<T>> resultTask,
        Action<T> action) => (await resultTask).Tap(action);

    // async_async => sync_async
    public static async Task<IResult<T>> Tap<T>(
        this Task<IResult<T>> resultTask,
        Func<T, Task> func) => await (await resultTask).Tap(func);

}
