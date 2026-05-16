using Bookly.Application.Abstractions.Clock;
using Bookly.Application.Abstractions.Data;
using Bookly.Application.Abstractions.Email;
using Bookly.Domain.Abstractions;
using Bookly.Domain.Apartments;
using Bookly.Domain.Bookings;
using Bookly.Domain.Users;
using Bookly.Infrastructure.Clock;
using Bookly.Infrastructure.Data;
using Bookly.Infrastructure.Email;
using Bookly.Infrastructure.Repositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;




namespace Bookly.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IEmailService, EmailService>();

            var connectionString =
                configuration.GetConnectionString("Database")
                ?? throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IApartmentRepository, ApartmentRepository>();

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddSingleton<ISqlConnectionFactory>(sp =>
                new SqlConnectionFactory(connectionString));

            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

            return services;
        }
    }
}
