using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Baz.Service.Action.Core;
using Microsoft.IdentityModel.Tokens;
using Yumi.Application.Configuration;
using Yumi.Application.Dto.Auth;

namespace Yumi.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly YumiConfiguration _yumiConfiguration;
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthenticationService(YumiConfiguration yumiConfiguration, IHttpClientFactory httpClientFactory)
    {
        _yumiConfiguration = yumiConfiguration;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IServiceActionResult<TokenDto>> AuthenticateAsync(
        string token,
        CancellationToken cancellationToken)
    {
        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"{_yumiConfiguration.GoogleUserProfileEndpoint}?access_token={token}");
        
        var client = _httpClientFactory.CreateClient();
        var response = await client.SendAsync(httpRequestMessage, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return ServiceActionResult<TokenDto>.Get(
                ServiceActionResponseNames.UnauthorizedAccess,
                "Error reading user profile");
        }

        var profile = await response
            .Content
            .ReadFromJsonAsync<GoogleUserProfileResponse>(cancellationToken: cancellationToken);
        
        if (profile == null ||
            !_yumiConfiguration.RespectedUserAccounts.Any(s => string.Equals(profile.Email, s)))
        {
            return ServiceActionResult<TokenDto>.Get(
                ServiceActionResponseNames.UnauthorizedAccess,
                "Invalid administrator account");
        }

        return ServiceActionResult<TokenDto>.Get(
            ServiceActionResponseNames.Success,
            GetApiToken(profile));
    }
    
    private TokenDto GetApiToken(GoogleUserProfileResponse userProfile)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_yumiConfiguration.TokenSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, userProfile.Email),
                new Claim("picture", userProfile.Picture)
            }),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return new TokenDto
        {
            Token = tokenString
        };
    }
}