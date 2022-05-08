using Baz.Service.Action.Core;
using Yumi.Application.Dto.Auth;

namespace Yumi.Application.Services;

public interface IAuthenticationService
{
    Task<IServiceActionResult<TokenDto>> AuthenticateAsync(string token, CancellationToken cancellationToken);
}