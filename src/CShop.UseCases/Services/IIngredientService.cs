using CShop.UseCases.Dtos;
using CShop.UseCases.UseCases.Commands.Ingredenents;
using CShop.UseCases.UseCases.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Services;
public interface IIngredientService
{
    Task<IEnumerable<IngredientDto>> GetIngredients();
    Task UpsertIngredient(IngredientDto ingredient);
    Task DeleteIngredient(int id);
    Task<IngredientDto> GetIngredient(int id);
}

internal class IngredientService(IServiceProvider sp) : BaseService(sp), IIngredientService
{
    public async Task DeleteIngredient(int id)
    {
        var mediator = mediatorModule.Value;
        await mediator.Send(new DeleteIngredientCommand(id));
    }

    public async Task<IngredientDto> GetIngredient(int id)
    {
        var mediator = mediatorModule.Value;
        return await mediator.Send(new GetIngredientQuery(id));
    }

    public async Task<IEnumerable<IngredientDto>> GetIngredients()
    {
        var mediator = mediatorModule.Value;
        var res = await mediator.Send(new GetIngredientsQuery());
        return res;
    }

    public async Task UpsertIngredient(IngredientDto ingredient)
    {
        var mediator = mediatorModule.Value;
        await mediator.Send(new UpsertIngredientCommand(ingredient));
    }
}
