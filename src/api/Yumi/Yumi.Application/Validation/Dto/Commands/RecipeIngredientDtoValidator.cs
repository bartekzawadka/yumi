using FluentValidation;
using Yumi.Application.Dto.Commands;

namespace Yumi.Application.Validation.Dto.Commands;

public class RecipeIngredientDtoValidator : AbstractValidator<RecipeIngredientDto>
{
    public RecipeIngredientDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().WithMessage("Name of ingredient is required");
        RuleFor(dto => dto.Unit).NotEmpty().WithMessage("Unit of ingredient is required");
    }
}