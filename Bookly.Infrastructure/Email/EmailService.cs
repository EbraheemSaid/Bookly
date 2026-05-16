using Bookly.Application.Abstractions.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Infrastructure.Email
{
    internal sealed class EmailService : IEmailService
    {
        public Task SendAsync(Domain.Users.Email recepient, string subject, string body)
        {
            return Task.CompletedTask;
        }
    }
}
