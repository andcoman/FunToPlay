using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text.Json;
using FunToPlay.Application.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace FunToPlay.Application.Utils;

public class MessageHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<string, Func<string, Task<object>>> _handlerMap = new();

    public MessageHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public void Register<TRequest, TResponse, THandler>(string messageType)
        where TRequest : class
        where TResponse : class
        where THandler : IHandler<TRequest, TResponse>
    {
        _handlerMap[messageType] = async (messageJson) =>
        {
            var handler = _serviceProvider.GetRequiredService<THandler>();
            var request = JsonSerializer.Deserialize<TRequest>(messageJson);
            var result = await handler.HandleAsync(request);

            return result;
        };
    }
    
    public async Task<object> ResolveAndInvokeAsync(string messageType, string messageJson)
    {
        if (!_handlerMap.TryGetValue(messageType, out var handlerFunc))
        {
            throw new InvalidOperationException($"No handler registered for message type '{messageType}'.");
        }
    
        return await handlerFunc(messageJson);
    }
}