using Baz.Service.Action.Core;
using Yumi.Application.Dto.Commands;
using Yumi.Application.Dto.Requests;
using Yumi.Application.Dto.Responses;
using Yumi.Infrastructure.Models;
using Yumi.Infrastructure.Repositories;
using Yumi.Infrastructure.Sys;

namespace Yumi.Application.Services;

public class RecipeService : IRecipeService
{
    private readonly IGenericRepository<Recipe> _recipeRepository;

    public RecipeService(IGenericRepository<Recipe> recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    public Task<PagedList<RecipeListItem>> GetListAsync(
        GetRecipiesListQuery query,
        CancellationToken cancellationToken) =>
        _recipeRepository.GetListAsAsync(
            query,
            recipe => new RecipeListItem
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                TimeStamp = recipe.TimeStamp,
                Photo = recipe.Photos.FirstOrDefault() ?? string.Empty,
                TotalIdleTime = recipe.RecipeSteps.Sum(step => step.IdleTimeInMinutes)
            },
            cancellationToken,
            recipe => recipe.Name, recipe => recipe.Description);

    public async Task<IServiceActionResult<Recipe>> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        if (!await _recipeRepository.ExistsAsync(recipe => recipe.Id == id, cancellationToken))
        {
            return ServiceActionResult<Recipe>.Get(
                ServiceActionResponseNames.ObjectNotFound,
                "Recipe with specified ID could not be found");
        }

        var result = await _recipeRepository.GetByIdAsync(id, cancellationToken);
        return ServiceActionResult<Recipe>.GetSuccess(result);
    }

    public async Task<IServiceActionResult> InsertAsync(
        CreateRecipeCommand command,
        CancellationToken cancellationToken)
    {
        var recipe = new Recipe
        {
            Description = command.Description,
            Id = Guid.NewGuid().ToString(),
            Name = command.Name,
            Photos = command.Photos,
            Ingredients = command.Ingredients.Select(dto => new RecipeIngredient
            {
                Amount = dto.Amount,
                Name = dto.Name,
                Unit = dto.Unit
            }).ToList(),
            RecipeSteps = command.RecipeSteps.Select(dto => new RecipeStep
            {
                Name = dto.Name,
                Content = dto.Content,
                IdleTimeInMinutes = dto.IdleTimeInMinutes
            }).ToList(),
            TimeStamp = DateTime.UtcNow
        };

        await _recipeRepository.InsertOrUpdateAsync(recipe, cancellationToken);
        await _recipeRepository.SaveChangesAsync(cancellationToken);
        
        return ServiceActionResult.Get(ServiceActionResponseNames.Created);
    }

    public async Task<IServiceActionResult> UpdateAsync(
        UpdateRecipeCommand command,
        CancellationToken cancellationToken)
    {
        if (!await _recipeRepository.ExistsAsync(recipe => recipe.Id == command.Id, cancellationToken))
        {
            return ServiceActionResult.Get(
                ServiceActionResponseNames.ObjectNotFound,
                "Recipe with specified ID could not be found");
        }

        var recipe = await _recipeRepository.GetByIdAsync(command.Id, cancellationToken);
        recipe.Description = command.Description;
        recipe.Name = command.Name;
        recipe.Ingredients = command.Ingredients.Select(dto => new RecipeIngredient
        {
            Amount = dto.Amount,
            Name = dto.Name,
            Unit = dto.Unit
        }).ToList();
        recipe.RecipeSteps = command.RecipeSteps.Select(dto => new RecipeStep
        {
            Name = dto.Name,
            Content = dto.Content,
            IdleTimeInMinutes = dto.IdleTimeInMinutes
        }).ToList();
        recipe.TimeStamp = DateTime.UtcNow;
        
        await _recipeRepository.InsertOrUpdateAsync(recipe, cancellationToken);
        await _recipeRepository.SaveChangesAsync(cancellationToken);
        
        return ServiceActionResult.Get(ServiceActionResponseNames.Success);
    }

    public async Task<IServiceActionResult> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        if (!await _recipeRepository.ExistsAsync(recipe => recipe.Id == id, cancellationToken))
        {
            return ServiceActionResult.Get(
                ServiceActionResponseNames.ObjectNotFound,
                "Recipe with specified ID could not be found");
        }
        
        _recipeRepository.Delete(id);
        await _recipeRepository.SaveChangesAsync(cancellationToken);

        return ServiceActionResult.Get(ServiceActionResponseNames.Success);
    }
}