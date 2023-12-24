namespace CShop.Domain.Primitives.Results;

public sealed record Error(string Code, string? Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}
