using TravelAgentWeb.Models;
using Microsoft.EntityFrameworkCore;
using TravelAgentWeb.Data;
using Microsoft.Extensions.DependencyInjection;

namespace TravelAgentWeb
{
    public static class StartUp
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            // Register the Travel Agent DB context using connection string "DefaultConnection"
            builder.Services.AddTravelAgentDbContext(builder.Configuration);

            // AutoMapper mappings can go here if needed
            // builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Other service registrations can go here if needed
            builder.Services.AddControllers();
        }

        public static void Configure(WebApplication app)
        {
            // Apply pending EF Core migrations on startup (if any)
            try
            {
                using var scope = app.Services.CreateScope();
                var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("StartUp");
                var db = scope.ServiceProvider.GetRequiredService<TravelAgentDbContext>();
                logger.LogInformation("Applying database migrations (if any)");
                db.Database.Migrate();
                logger.LogInformation("Database migrations applied");
            }
            catch (Exception ex)
            {
                var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("StartUp");
                logger.LogError(ex, "An error occurred while migrating or initializing the database.");
                throw; // rethrow so the app doesn't start in a bad state
            }
        }
    }
}