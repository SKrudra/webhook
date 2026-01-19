using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelAgentWeb.Models;

namespace TravelAgentWeb.Data
{

    /// <summary>
    /// EF Core DB context for the Travel Agent application.
    /// Contains DbSets for application entities and model configuration.
    /// </summary>
    public class TravelAgentDbContext : DbContext
    {
        public TravelAgentDbContext(DbContextOptions<TravelAgentDbContext> options) : base(options)
        {
        }

        public DbSet<WebhookSecret> WebhookSecrets { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WebhookSecret>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Secret).IsRequired();
                entity.Property(e => e.Publisher).IsRequired();
            });
        }

    }

    public static class TravelAgentDbContextExtensions
    {
        public static IServiceCollection AddTravelAgentDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var conn = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Missing connection string 'DefaultConnection'");
            services.AddDbContext<TravelAgentDbContext>(options => options.UseSqlServer(conn));
            return services;
        }
    }

}