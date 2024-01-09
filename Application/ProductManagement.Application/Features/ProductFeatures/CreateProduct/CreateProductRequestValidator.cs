using FluentValidation;

namespace ProductManagement.Application.Features.ProductFeatures.CreateProduct;

/// <summary>
/// Validations for create product feature.
/// Is sealed because we don't want anything else to inherit from it. Main reason is it can change in the future.
/// </summary>
public sealed class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .Length(3, 50);
        RuleFor(x => x.Description)
            .MaximumLength(500);
        RuleFor(x => x.Quantity)
            .NotNull()
           .GreaterThanOrEqualTo(0);
    }
}
