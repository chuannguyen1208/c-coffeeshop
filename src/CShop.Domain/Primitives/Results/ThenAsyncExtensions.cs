namespace CShop.Domain.Primitives.Results;
public static class ThenAsyncExtensions
{
    public static async Task<IResult> Then(
        this Task task,
        Func<Task> func)
    {
        await task.ConfigureAwait(false);
        await func();

        return Result.Success;
    }

    public static async Task<IResult<T>> Then<T>(
        this Task task,
        Func<Task<T>> func)
    {
        await task.ConfigureAwait(false);
        var value = await func();

        return Result<T>.Success(value);
    }

    public static async Task<IResult<T>> Then<TIn, T>(
        this Task<TIn> task,
        Func<TIn, Task<T>> func)
    {
        var tin = await task.ConfigureAwait(false);
        var value = await func(tin);

        return Result<T>.Success(value);
    }
}
