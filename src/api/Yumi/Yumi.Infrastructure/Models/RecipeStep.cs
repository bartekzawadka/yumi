namespace Yumi.Infrastructure.Models;

public class RecipeStep
{
    public string Name { get; set; } = string.Empty;
    
    public string Content { get; set; } = string.Empty;

    public int IdleTimeInMinutes { get; set; }
}