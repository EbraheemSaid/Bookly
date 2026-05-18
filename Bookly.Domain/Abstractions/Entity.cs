using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Domain.Abstractions
{
    public abstract class Entity
    {
        // This list will hold the domain events that are associated with this entity. It is private, so it can only be accessed from within the class.
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        protected Entity(Guid id)
        {
            Id = id;

        }
        protected Entity()
        {

        }
        public Guid Id { get; init; }

        public IReadOnlyList<IDomainEvent> GetDomainEvents()
        {
            return _domainEvents.ToList();
        }
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
        // This method is protected, so it can only be called from within the class or its subclasses.
        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }

}
