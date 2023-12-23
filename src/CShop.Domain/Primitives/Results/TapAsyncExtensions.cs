namespace CShop.Domain.Primitives.Results;
public static class TapAsyncExtensions
{
    public static async Task<IResult<T>> Tap<T>(
        this Task<T> task,
        Func<Task> func) => await task.Then(async tin =>
        {
            await func();
            return tin;
        });
}
