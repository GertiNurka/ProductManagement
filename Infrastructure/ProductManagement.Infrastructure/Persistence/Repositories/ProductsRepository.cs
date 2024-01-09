using Microsoft.Extensions.Logging;
using ProductManagement.Application.Common.Interfaces.Persistence;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Entities.Validators;
using ProductManagement.Infrastructure.Persistence.Context;

namespace ProductManagement.Infrastructure.Persistence.Repositories;

public class ProductsRepository : BaseRepository<Product>, IProductsRepository
{
    private ILogger<ProductsRepository> _logger;

    public ProductsRepository(DataContext context, ILogger<ProductsRepository> logger, ILogger<BaseRepository<Product>> baseLogger) 
        : base(context, baseLogger, new ProductValidator())
    {
        _logger = logger;
    }

    //TODO: Add more methods based on use cases.
}
