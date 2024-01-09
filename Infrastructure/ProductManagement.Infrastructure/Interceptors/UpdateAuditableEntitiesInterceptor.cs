using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProductManagement.Domain.Common.Audit;

namespace ProductManagement.Infrastructure.Interceptors;

/// <summary>
/// Interceptor used for auditing.
///     TODO: 
///         1.  Extract this in its own 'Auditing' library. 
///         2.  Log all changes in an 'Audit' model and then dispatch it back to application layer.
///             From there the application layer can persist the audit in its own database/table or it can send it to a centralized service that handles audits.
///             For example IAuditsService.CreateAudit(audits)
/// </summary>
public sealed class UpdateAuditableEntitiesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, 
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if(dbContext == null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = dbContext.ChangeTracker.Entries<IAuditable>();

        foreach (var entityEntry in entries )
        {
            if(entityEntry.State == EntityState.Added)
                entityEntry.Property(x => x.CreatedOnUtc).CurrentValue = DateTimeOffset.UtcNow;
            if(entityEntry.State == EntityState.Modified)
                entityEntry.Property(x => x.UpdatedOnUtc).CurrentValue = DateTimeOffset.UtcNow;
            if (entityEntry.State == EntityState.Deleted)
                entityEntry.Property(x => x.DeletedOnUtc).CurrentValue = DateTimeOffset.UtcNow;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
