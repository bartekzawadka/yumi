namespace Yumi.Infrastructure.Models;

public class DbDocument
{
    public string Id { get; set; } = string.Empty;

    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
}