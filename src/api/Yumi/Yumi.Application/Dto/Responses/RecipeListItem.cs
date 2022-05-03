namespace Yumi.Application.Dto.Responses;

public record RecipeListItem
{
    public string Id { get; set; } = string.Empty;
    
    public string Name { get; init; } = string.Empty;

    public DateTime TimeStamp { get; init; } = DateTime.UtcNow;

    public string Description { get; init; } = string.Empty;

    public string Photo { get; init; } = string.Empty;

    public int TotalIdleTime { get; init; } = 0;
}