using System.ComponentModel.DataAnnotations;

namespace Yumi.Application.Configuration;

public class YumiConfiguration
{
    [Required]
    public string AllowedHost { get; set; } = string.Empty;

    [Required]
    public string TokenSecret { get; set; } = string.Empty;

    public ICollection<string> RespectedUserAccounts { get; set; } = new List<string>();

    [Required] public string GoogleUserProfileEndpoint { get; set; } = string.Empty;
}