namespace Yumi.Application.Dto.Commands;

public record UpdateRecipeCommand : CreateRecipeCommand
{
    public string Id { get; init; } = string.Empty;
}