using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Application.Services.Products;
using System.Reflection;

namespace ProductManagement.Application.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<IProductsService, ProductsService>();

        return services;
    }
}
