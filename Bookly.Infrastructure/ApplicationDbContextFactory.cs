using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Bookly.Infrastructure
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Connection string for migrations (runs from host machine, not Docker)
            // Points to localhost:5432 which maps to the Docker PostgreSQL container
            var connectionString = "Host=localhost;Port=5432;Database=bookly;Username=postgres;Password=postgres";

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();

            // Create a dummy publisher for design-time
            var publisher = new NoOpPublisher();

            return new ApplicationDbContext(optionsBuilder.Options, publisher);
        }

        private class NoOpPublisher : IPublisher
        {
            public Task Publish(object notification, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }

            public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
            {
                return Task.CompletedTask;
            }
        }
    }
}
