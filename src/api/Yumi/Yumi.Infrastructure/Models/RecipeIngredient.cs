namespace Yumi.Infrastructure.Models;

public class RecipeIngredient
{
    public string Name { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Unit { get; set; } = string.Empty;
}