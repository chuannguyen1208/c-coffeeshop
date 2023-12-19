namespace CShop.Domain.Primitives;

public class Entity(Guid id)
{
    public Guid Id { get; private set; } = id;
}
