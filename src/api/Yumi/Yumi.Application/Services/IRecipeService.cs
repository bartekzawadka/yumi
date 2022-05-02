using Yumi.Application.Dto.Requests;
using Yumi.Application.Dto.Responses;
using Yumi.Infrastructure.Sys;

namespace Yumi.Application.Services;

public interface IRecipeService
{
    Task<PagedList<RecipeListItem>> GetListAsync(
        GetRecipiesListQuery query,
        CancellationToken cancellationToken);
}