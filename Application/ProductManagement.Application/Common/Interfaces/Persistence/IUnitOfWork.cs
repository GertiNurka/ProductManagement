namespace ProductManagement.Application.Common.Interfaces.Persistence;

public interface IUnitOfWork
{
    Task Save(CancellationToken cancellationToken);
}
