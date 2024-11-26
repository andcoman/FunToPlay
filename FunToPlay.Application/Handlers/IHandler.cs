using System.Net.WebSockets;
using FunToPlay.Application.Session;
using FunToPlay.Domain.Utils;

namespace FunToPlay.Application.Handlers;

public interface IHandler<TMessage, TResponse>
    where TMessage : class
    where TResponse : class
{
    Task<OperationResult<TResponse>> HandleAsync(TMessage message, CancellationToken cancellationToken = default);
}
