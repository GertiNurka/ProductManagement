using FluentValidation;

namespace ProductManagement.Application.Features.ProductFeatures.UpdateProduct;

public sealed class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .Length(3, 50);
        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
