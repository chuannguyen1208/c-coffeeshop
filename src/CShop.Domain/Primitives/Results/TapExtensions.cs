namespace CShop.Domain.Primitives.Results;
public static class TapExtensions
{
    public static async Task<IResult<T>> Tap<T>(
        this Task<IResult<T>> result,
        Func<Task> func)
    {
        await func();
        return await result.ConfigureAwait(false);
    }
}
