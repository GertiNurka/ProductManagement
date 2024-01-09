using ProductManagement.Domain.Common;

namespace ProductManagement.Application.Common.Interfaces.Persistence;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task Create(T entity);
    Task Update(T entity);
    void Delete(T entity);
    Task<T?> Get(int id, CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken);
}
