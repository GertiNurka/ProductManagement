using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Application.Common.Interfaces.Email;
using ProductManagement.Application.Common.Interfaces.Persistence;
using ProductManagement.Infrastructure.Email;
using ProductManagement.Infrastructure.Interceptors;
using ProductManagement.Infrastructure.Persistence.Context;
using ProductManagement.Infrastructure.Persistence.Repositories;

namespace ProductManagement.Infrastructure.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        //Add interceptor for auditing
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

        //For 
        services.AddDbContext<DataContext>((sp, opt) =>
        {
            var auditableEntitiesInterceptor = sp.GetService<UpdateAuditableEntitiesInterceptor>()!;

            opt
            .UseInMemoryDatabase(databaseName: "LocalDatabase")
            .AddInterceptors(auditableEntitiesInterceptor);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductsRepository, ProductsRepository>();

        services.AddScoped<IProductsEmailService, ProductsEmailService>();
        services.AddScoped<ISmtpClientHelper, SmtpClientHelper>();

        return services;
    }
}
