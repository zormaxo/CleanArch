using CleanArch.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CleanArch.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var logger = scope
            .ServiceProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger(nameof(ApplicationDbContextInitialiser));
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await ApplicationDbContextInitialiser.InitialiseAsync(context, logger);
        await ApplicationDbContextInitialiser.SeedAsync(context, logger);
    }
}

public static class ApplicationDbContextInitialiser
{
    public static async Task InitialiseAsync(ApplicationDbContext context, ILogger logger)
    {
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public static async Task SeedAsync(ApplicationDbContext context, ILogger logger)
    {
        try
        {
            await TrySeedAsync(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private static async Task TrySeedAsync(ApplicationDbContext context)
    {
        // Default data
        // Seed, if necessary
        if (!context.TodoLists.Any())
        {
            context.TodoLists.Add(
                new TodoList
                {
                    Title = "Todo List",
                    Items =
                    {
                        new TodoItem { Title = "Make a todo list 📃" },
                        new TodoItem { Title = "Check off the first item ✅" },
                        new TodoItem
                        {
                            Title = "Realise you've already done two things on the list! 🤯",
                        },
                        new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
                    },
                }
            );

            await context.SaveChangesAsync();
        }
    }
}
