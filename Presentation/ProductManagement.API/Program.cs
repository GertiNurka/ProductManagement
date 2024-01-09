using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using ProductManagement.Application.Configuration;
using ProductManagement.Application.Features.ProductFeatures.CreateProduct;
using ProductManagement.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        //Inject Infrastructure layer
        .AddInfrastructure()
        //Inject application layer
        .AddApplication();

    builder.Services.AddControllers();

    //Add authorization
    //TODO:
    //  Uncomment to enable only authorized users to log in for test/live environments
    //  Two polices are defined Admin and User
    //builder.Services.AddAuthorization(options =>
    //{
    //    //Admin policy
    //    options.AddPolicy("Admin", policy =>
    //    {
    //        //Must have admin role
    //        policy.RequireClaim("roles", "Admin");

    //        //Must have api-productManagement scope
    //        //Using 'RequireAssertion' because usually scopes are passed as string separated with spaces. If scopes are passed as arrays then 'RequireClaim' can be used.
    //        policy.RequireAssertion(context =>
    //        {
    //            var claim = context.User.FindFirst("scp");
    //            if (claim == null)
    //                return false;

    //            return claim.Value.Split(' ').Any(scope => scope == "api-productManagement");
    //        });
    //    });

    //    //User policy
    //    options.AddPolicy("User", policy =>
    //    {
    //        //Must have user role
    //        policy.RequireClaim("roles", "User");

    //        //Must have api-productManagement scope
    //        //Using 'RequireAssertion' because usually scopes are passed as string separated with spaces. If scopes are passed as arrays then 'RequireClaim' can be used.
    //        policy.RequireAssertion(context =>
    //        {
    //            var claim = context.User.FindFirst("scp");
    //            if (claim == null)
    //                return false;

    //            return claim.Value.Split(' ').Any(scope => scope == "api-productManagement");
    //        });
    //    });
    //});

    //Adding fluent validation
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        //Swagger authorization
        //TODO: Uncomment to enable swagger authorization for test/live environments
        //options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        //{
        //    Type = SecuritySchemeType.OAuth2,
        //    Flows = new OpenApiOAuthFlows
        //    {
        //        //Chose your preferred flow if implicit is not fit in your case
        //        Implicit = new OpenApiOAuthFlow
        //        {
        //            AuthorizationUrl = new Uri($"YourIdentityUrl/connect/authorize"),
        //            TokenUrl = new Uri($"YourIdentityUrl/connect/token"),
        //            Scopes = new Dictionary<string, string>
        //                    {
        //                        { "api-productManagement", "Product Management API" }
        //                    }
        //        }
        //    }
        //});
    });

    //Add authentication to validate token
    //TODO: Uncomment this for test/live environments
    //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //            .AddJwtBearer(options =>
    //            {
    //                options.Authority = "YourAuthorityUrl";
    //                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    //                {
    //                    ValidateIssuer = true,
    //                    ValidAudiences = new List<string>
    //                    {
    //                        "YourAudienceId"
    //                    }
    //                };
    //            });
}


var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //Set global error handler endpoint
    app.UseExceptionHandler("/error");

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
