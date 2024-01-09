namespace ProductManagement.Domain.Common.Audit;

/// <summary>
/// Defines which entities will be auditable.
/// TODO: Extract this to its own 'Auditing' library.
/// </summary>
public interface IAuditable
{
    DateTimeOffset CreatedOnUtc { get; set; }
    DateTimeOffset? UpdatedOnUtc { get; set; }
    DateTimeOffset? DeletedOnUtc { get; set; }
}
