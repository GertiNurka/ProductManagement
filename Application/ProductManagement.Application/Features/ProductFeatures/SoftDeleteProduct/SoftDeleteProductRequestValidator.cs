using FluentValidation;

namespace ProductManagement.Application.Features.ProductFeatures.SoftDeleteProduct;

public sealed class SoftDeleteProductRequestValidator : AbstractValidator<SoftDeleteProductRequest>
{
    public SoftDeleteProductRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(1);
    }
}
