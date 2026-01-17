using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AirlineWeb.Data;
using AutoMapper;
using AirlineWeb.Mapping;

namespace AirlineWeb
{
    /// <summary>
    /// Central startup helpers to register services and perform app initialization.
    /// Kept separate to keep `Program.cs` minimal.
    /// </summary>
    public static class StartUp
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            // Register the Airline DB context using connection string "DefaultConnection"
            builder.Services.AddAirlineDbContext(builder.Configuration);

            // AutoMapper mappings
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Other service registrations can go here if needed
            builder.Services.AddControllers();
        }

        public static void Configure(WebApplication app)
        {
            // Apply pending EF Core migrations on startup (if any)
            try
            {
                app.UseStaticFiles();
                using var scope = app.Services.CreateScope();
                var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("StartUp");
                var db = scope.ServiceProvider.GetRequiredService<AirlineDbContext>();
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
