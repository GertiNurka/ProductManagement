using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.Common.Interfaces.Persistence;
using ProductManagement.Domain.Common;
using ProductManagement.Infrastructure.Persistence.Context;

namespace ProductManagement.Infrastructure.Persistence.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DataContext _context;
    private readonly DbSet<TEntity> _entity;
    private readonly ILogger<BaseRepository<TEntity>> _logger;
    private readonly AbstractValidator<TEntity> _validator;

    public BaseRepository(DataContext context, ILogger<BaseRepository<TEntity>> logger, AbstractValidator<TEntity> validator)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _entity = _context.Set<TEntity>();
        _logger = logger;
        _validator = validator;

        _logger.LogInformation($"{nameof(BaseRepository<TEntity>)} created.");
    }

    public async Task Create(TEntity entity)
    {
        _logger.LogInformation($"{nameof(BaseRepository<TEntity>.Create)} creating {typeof(TEntity).Name}.");

        await ValidateEntity(entity);

        _entity.Add(entity);

        _logger.LogInformation($"{nameof(BaseRepository<TEntity>.Create)} created {typeof(TEntity).Name}.");
    }

    public async Task Update(TEntity entity)
    {
        _logger.LogInformation($"{nameof(BaseRepository<TEntity>.Update)} updating {typeof(TEntity).Name}.");

        await ValidateEntity(entity);

        _entity.Update(entity);

        _logger.LogInformation($"{nameof(BaseRepository<TEntity>.Update)} updated {typeof(TEntity).Name}.");
    }

    public void Delete(TEntity entity)
    {
        _logger.LogInformation($"{nameof(BaseRepository<TEntity>.Delete)} deleting {typeof(TEntity).Name}.");

        _entity.Remove(entity);

        _logger.LogInformation($"{nameof(BaseRepository<TEntity>.Delete)} deleted {typeof(TEntity).Name}.");
    }

    public async Task<TEntity?> Get(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(BaseRepository<TEntity>.Get)} retrieving {typeof(TEntity).Name} with id: {id}.");

        var entity = await _entity.FindAsync(id, cancellationToken);

        _logger.LogInformation($"{nameof(BaseRepository<TEntity>.Get)} retrieved {typeof(TEntity).Name} with id: {id}.");

        return entity;
    }

    public async Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(BaseRepository<TEntity>.GetAll)} retrieving all {typeof(TEntity).Name}s.");

        var entities = await _entity.ToListAsync(cancellationToken);

        _logger.LogInformation($"{nameof(BaseRepository<TEntity>.Get)} retrieved all {typeof(TEntity).Name}s.");

        return entities;
    }

    /// <summary>
    /// Used as last validation before persisting
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    protected virtual async Task ValidateEntity(TEntity entity)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(entity);

        if (!validationResult.IsValid)
        {
            var message = "";

            foreach (var error in validationResult.Errors)
            {
                message = $"{message}Error: {error.ErrorMessage}";
            }

            throw new ValidationException(message);
        }
    }
}
