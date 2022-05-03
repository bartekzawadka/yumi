using FluentValidation;
using Yumi.Application.Dto.Commands;

namespace Yumi.Application.Validation.Dto.Commands;

public class RecipeStepDtoValidator : AbstractValidator<RecipeStepDto>
{
    public RecipeStepDtoValidator()
    {
        RuleFor(dto => dto.Content).NotEmpty().WithMessage("Recipe step description is required");
    }
}