using Bookly.Domain.Abstractions;
using Bookly.Domain.Users.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Domain.Users
{
    public sealed class User : Entity
    {
        public FirstName FirstName { get; private set; }
        public LastName LastName { get; private set; }
        public Email Email { get; private set; }

        private User(Guid id, FirstName firstName, LastName lastName, Email email) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public static User Create(FirstName firstName, LastName lastName, Email email)
        {
            var user = new User(Guid.NewGuid(), firstName, lastName, email);

            // side effects that dont naturally fit in the constructor, such as: Domain events, validations, etc.

            var userCreatedEvent = new UserCreatedDomainEvent(user.Id);
            // someone can subscribe to this event and do something when a user is created, such as: send a welcome email, log the creation, etc.
            user.RaiseDomainEvent(userCreatedEvent);

            return user;
        }
    }
}
