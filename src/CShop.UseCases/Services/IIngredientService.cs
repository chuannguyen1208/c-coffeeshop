using CShop.UseCases.Dtos;
using CShop.UseCases.UseCases.Commands.Ingredenents;
using CShop.UseCases.UseCases.Queries.Ingredients;

using MediatR;

namespace CShop.UseCases.Services;
public interface IIngredientService
{
    Task<IEnumerable<IngredientDto>> GetIngredients();
    Task CreateIngredient(IngredientDto ingredient);
    Task UpdateIngredient(IngredientDto ingredient);
    Task DeleteIngredient(Guid id);
    Task<IngredientDto> GetIngredient(Guid id);
}

internal class IngredientService(IMediator mediator) : IIngredientService
{
    public async Task DeleteIngredient(Guid id)
    {
        await mediator.Send(new DeleteIngredientCommand(id));
    }

    public async Task<IngredientDto> GetIngredient(Guid id)
    {
        return await mediator.Send(new GetIngredientQuery(id));
    }

    public async Task<IEnumerable<IngredientDto>> GetIngredients()
    {
        var res = await mediator.Send(new GetIngredientsQuery());
        return res;
    }

    public async Task CreateIngredient(IngredientDto ingredient)
    {
        await mediator.Send(new CreateIngredientCommand(ingredient));
    }

    public async Task UpdateIngredient(IngredientDto ingredient)
    {
        await mediator.Send(new UpdateIngredientCommand(ingredient));
    }
}
