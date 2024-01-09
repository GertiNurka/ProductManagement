using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Infrastructure.Persistence.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            options.UseInMemoryDatabase(databaseName: "LocalDatabase");
        }
    }

    /// <summary>
    /// Sets the configuration of product entity
    /// Commented out because of in-memory database.
    /// TODO: For a real database please uncomment and run migrations.
    /// </summary>
    /// <param name="builder"></param>
    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    builder.ApplyConfiguration(new ProductConfiguration());
    //}
}

