using Bookly.Application.Exceptions;
using Bookly.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Infrastructure
{
    public sealed class ApplicationDbContext : DbContext, IUnitOfWork
    {

        private readonly IPublisher _publisher;
        public ApplicationDbContext(DbContextOptions options, IPublisher publisher) : base(options)
        {
            _publisher = publisher;
        }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await base.SaveChangesAsync(cancellationToken);

                await PublishDomainEventsAsync();

                return result;
            }
            catch (DbUpdateConcurrencyException ex) // not leaking any efcore into the application layer
            {

                throw new ConcurrencyException("A concurrency error occurred.", ex);
            }

        }

        private async Task PublishDomainEventsAsync()
        {
            var domainEvents = ChangeTracker
                .Entries<Entity>()
                .Select(x => x.Entity)
                .SelectMany(entity =>
                {
                    var domainEvents = entity.GetDomainEvents();

                    entity.ClearDomainEvents();

                    return domainEvents;
                })
                .ToList();


            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent);
            }
        }
    }

}