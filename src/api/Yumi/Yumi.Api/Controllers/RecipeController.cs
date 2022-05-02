using Microsoft.AspNetCore.Mvc;
using Yumi.Application.Dto.Requests;
using Yumi.Application.Dto.Responses;
using Yumi.Application.Services;
using Yumi.Infrastructure.Sys;

namespace Yumi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _recipeService;

    public RecipeController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    [HttpPost("search")]
    public Task<PagedList<RecipeListItem>> GetListAsync(
        [FromBody] GetRecipiesListQuery query,
        CancellationToken cancellationToken) =>
        _recipeService.GetListAsync(query, cancellationToken);
}