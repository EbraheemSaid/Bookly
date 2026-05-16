using Bookly.Domain.Abstractions;
using MediatR;

namespace Bookly.Application.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
