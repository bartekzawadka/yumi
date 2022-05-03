using Baz.Service.Action.Core;
using Yumi.Application.Dto.Commands;
using Yumi.Application.Dto.Requests;
using Yumi.Application.Dto.Responses;
using Yumi.Infrastructure.Models;
using Yumi.Infrastructure.Sys;

namespace Yumi.Application.Services;

public interface IRecipeService
{
    Task<PagedList<RecipeListItem>> GetListAsync(
        GetRecipiesListQuery query,
        CancellationToken cancellationToken);

    Task<IServiceActionResult<Recipe>> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task<IServiceActionResult> InsertAsync(
        CreateRecipeCommand command,
        CancellationToken cancellationToken);

    Task<IServiceActionResult> UpdateAsync(
        UpdateRecipeCommand command,
        CancellationToken cancellationToken);

    Task<IServiceActionResult> DeleteAsync(string id, CancellationToken cancellationToken);
}