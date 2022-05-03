using Baz.Service.Action.Core;
using Microsoft.AspNetCore.Mvc;
using Yumi.Application.Dto.Commands;
using Yumi.Application.Dto.Requests;
using Yumi.Application.Dto.Responses;
using Yumi.Application.Services;
using Yumi.Infrastructure.Models;
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

    [HttpGet("{id}")]
    public Task<IServiceActionResult<Recipe>> GetByIdAsync(string id, CancellationToken cancellationToken) =>
        _recipeService.GetByIdAsync(id, cancellationToken);

    [HttpPost]
    public Task<IServiceActionResult> InsertAsync(
        [FromBody] CreateRecipeCommand command,
        CancellationToken cancellationToken) =>
        _recipeService.InsertAsync(command, cancellationToken);

    [HttpPut]
    public Task<IServiceActionResult> UpdateAsync(
        [FromBody] UpdateRecipeCommand command,
        CancellationToken cancellationToken) =>
        _recipeService.UpdateAsync(command, cancellationToken);

    [HttpDelete("{id}")]
    public Task<IServiceActionResult> DeleteAsync(string id, CancellationToken cancellationToken) =>
        _recipeService.DeleteAsync(id, cancellationToken);
}