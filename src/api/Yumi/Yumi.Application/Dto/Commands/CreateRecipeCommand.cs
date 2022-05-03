namespace Yumi.Application.Dto.Commands;

public record CreateRecipeCommand
{
    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public ICollection<string> Photos { get; init; } = new List<string>();

    public ICollection<RecipeIngredientDto> Ingredients { get; init; } = new List<RecipeIngredientDto>();

    public ICollection<RecipeStepDto> RecipeSteps { get; init; } = new List<RecipeStepDto>();
}