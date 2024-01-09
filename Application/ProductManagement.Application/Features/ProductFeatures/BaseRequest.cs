using FluentValidation;

namespace ProductManagement.Application.Features.ProductFeatures;

public abstract class BaseRequest<TValidator, TRequest>
    where TValidator : AbstractValidator<TRequest>, new()
    where TRequest : class
{
    public virtual void Validate()
    {
        var validator = new TValidator();

        var instance = this as TRequest;

        if (instance == null)
            throw new Exception($"Validator: '{typeof(TValidator).Name}' is not suitable to validate request: '{typeof(TRequest).Name}'");

        validator.ValidateAndThrow(instance);
    }

    public virtual (bool, string?) IsValid()
    {
        var validator = new TValidator();
        var instance = this as TRequest;

        if (instance == null)
            throw new Exception($"Validator: '{typeof(TValidator).Name}' is not suitable to validate request: '{typeof(TRequest).Name}'");

        var result = validator.Validate((this as TRequest)!);

        if (!result.IsValid)
        {
            var errors = string.Join(", ", result.Errors);
            return (false, errors);
        }

        return (true, null);
    }
}
