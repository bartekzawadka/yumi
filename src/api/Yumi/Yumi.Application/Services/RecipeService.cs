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
                Name = recipe.Name,
                Description = recipe.Description,
                TimeStamp = recipe.TimeStamp,
                Photo = recipe.Photos.FirstOrDefault() ?? string.Empty,
                TotalIdleTime = recipe.RecipeSteps.Sum(step => step.IdleTimeInMinutes)
            },
            cancellationToken,
            recipe => recipe.Name, recipe => recipe.Description);
}