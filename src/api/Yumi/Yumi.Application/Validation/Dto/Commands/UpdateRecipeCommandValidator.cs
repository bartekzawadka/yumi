using FluentValidation;
using Yumi.Application.Dto.Commands;

namespace Yumi.Application.Validation.Dto.Commands;

public class UpdateRecipeCommandValidator : AbstractValidator<UpdateRecipeCommand>
{
    public UpdateRecipeCommandValidator()
    {
        RuleFor(command => command.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(command => command.Ingredients).NotEmpty().WithMessage("At least one ingredient is required");
        RuleFor(command => command.Photos).NotEmpty().WithMessage("At least one image is required");
        RuleFor(command => command.RecipeSteps).NotEmpty().WithMessage("At least one step is required");
        RuleFor(command => command.Id).NotEmpty().WithMessage("Id of updated recipe is required");
    }
}