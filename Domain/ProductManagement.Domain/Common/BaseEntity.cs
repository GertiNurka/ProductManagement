using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Domain.Common;

public abstract class BaseEntity
{
    [Key]
    public virtual int? Id { get; protected init; }
}
