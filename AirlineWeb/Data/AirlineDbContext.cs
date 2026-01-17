using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AirlineWeb.Models;

namespace AirlineWeb.Data
{
    /// <summary>
    /// EF Core DB context for the Airline application.
    /// Contains DbSets for application entities and model configuration.
    /// </summary>
    public class AirlineDbContext : DbContext
    {
        public AirlineDbContext(DbContextOptions<AirlineDbContext> options)
            : base(options)
        {
        }

        public DbSet<WebhookSubscription> WebhookSubscriptions { get; set; } = null!;
        public DbSet<FlightDetails> FlightDetails { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WebhookSubscription>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.WebhookUrl).IsRequired();
                entity.Property(e => e.Secret).IsRequired();
                entity.Property(e => e.WebhookType).IsRequired();
                entity.Property(e => e.WebhookPublisher).IsRequired();
            });

            modelBuilder.Entity<FlightDetails>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FlightNumber).IsRequired();
                entity.Property(e => e.Price)
                      .HasColumnType("decimal(6,2)")
                      .IsRequired();
            });
        }
    }

    /// <summary>
    /// Helper extension to register the AirlineDbContext with DI using a connection string named "DefaultConnection".
    /// </summary>
    public static class AirlineDbContextExtensions
    {
        public static IServiceCollection AddAirlineDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var conn = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Missing connection string 'DefaultConnection'");
            services.AddDbContext<AirlineDbContext>(options => options.UseSqlServer(conn));
            return services;
        }
    }
}