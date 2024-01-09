using ProductManagement.Application.Common.Interfaces.Persistence;
using ProductManagement.Infrastructure.Persistence.Context;

namespace ProductManagement.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;

    public UnitOfWork(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task Save(CancellationToken cancellationToken)
    {
        await _dataContext.SaveChangesAsync(cancellationToken);
    }
}
