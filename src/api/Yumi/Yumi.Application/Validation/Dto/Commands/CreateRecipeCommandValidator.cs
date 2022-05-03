using FluentValidation;
using Yumi.Application.Dto.Commands;

namespace Yumi.Application.Validation.Dto.Commands;

public class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
{
    public CreateRecipeCommandValidator()
    {
        RuleFor(command => command.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(command => command.Ingredients).NotEmpty().WithMessage("At least one ingredient is required");
        RuleFor(command => command.Photos).NotEmpty().WithMessage("At least one image is required");
        RuleFor(command => command.RecipeSteps).NotEmpty().WithMessage("At least one step is required");
    }
}