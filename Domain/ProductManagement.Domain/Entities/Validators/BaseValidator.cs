using FluentValidation;
using ProductManagement.Domain.Common;

namespace ProductManagement.Domain.Entities.Validators;

public abstract class BaseValidator<TEntity> : AbstractValidator<TEntity> where TEntity : BaseEntity
{
    protected BaseValidator()
    {
        //Add base validations here as the requirements are growing
    }
}
