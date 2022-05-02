namespace Yumi.Infrastructure.Models;

public class Recipe : DbDocument
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ICollection<string> Photos { get; set; } = new List<string>();

    public ICollection<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();

    public ICollection<RecipeStep> RecipeSteps { get; set; } = new List<RecipeStep>();
}