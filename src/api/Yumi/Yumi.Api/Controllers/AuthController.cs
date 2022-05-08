using Baz.Service.Action.Core;
using Microsoft.AspNetCore.Mvc;
using Yumi.Application.Dto.Auth;
using Yumi.Application.Services;

namespace Yumi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    
    [HttpGet]
    public Task<IServiceActionResult<TokenDto>> AuthorizeAsync(string token, CancellationToken cancellationToken) => 
        _authenticationService.AuthenticateAsync(token, cancellationToken);    
}