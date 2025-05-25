using CleanArch.Infrastructure.Data;
using CleanArch.Application.Common.Interfaces;
using CleanArch.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace CleanArch.Web;

public static class DependencyInjection
{
    public static void AddWebServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUser, CurrentUser>();
        builder.Services.AddHttpContextAccessor();

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        }

        builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        // Customise default API behaviour
        builder.Services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true
        );

        // This is to ensure that the API documentation is generated correctly for minimal APIs
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddOpenApiDocument(
            (configure, sp) =>
            {
                configure.Title = "CleanArch API";

                // Add JWT
                configure.AddSecurity(
                    "JWT",
                    [],
                    new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Description = "Type into the textbox: Bearer {your JWT token}.",
                    }
                );

                configure.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("JWT")
                );
            }
        );
    }
}
