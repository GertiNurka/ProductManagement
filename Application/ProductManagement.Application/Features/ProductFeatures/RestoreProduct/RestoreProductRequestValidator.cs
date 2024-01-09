using FluentValidation;

namespace ProductManagement.Application.Features.ProductFeatures.RestoreProduct;

public sealed class RestoreProductRequestValidator : AbstractValidator<RestoreProductRequest>
{
    public RestoreProductRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(1);
    }
}
