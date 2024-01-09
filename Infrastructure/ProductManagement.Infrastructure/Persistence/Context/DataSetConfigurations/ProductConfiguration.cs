using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Infrastructure.Persistence.Context.DataSetConfigurations;

internal class ProductConfiguration : BaseConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(500);

        //Ignore as these are valid only at runtime and not to be persisted
        builder.Ignore(x => x.NewInStock);
        builder.Ignore(x => x.NewOutOfStock);
    }
}