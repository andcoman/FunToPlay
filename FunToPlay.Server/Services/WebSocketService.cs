using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using FunToPlay.Application.Messages;
using FunToPlay.Application.Session;
using FunToPlay.Application.Utils;
using FunToPlay.Domain.Repositories;

namespace FunToPlay.Server.Services;

public class WebSocketService
{
    private readonly  MessageHandler _messageHandler;
    private readonly SessionTracker _sessionTracker;
    
    public WebSocketService(MessageHandler messageHandler)
    {
        _messageHandler = messageHandler;
    }

    public async Task StartAsync(string address)
    {
        var httpListener = new HttpListener();
        httpListener.Prefixes.Add(address);
        httpListener.Start();
        
        while (true)
        {
            var context = await httpListener.GetContextAsync();

            if (context.Request.IsWebSocketRequest)
            {
                var webSocketContext = await context.AcceptWebSocketAsync(null);
                await HandleConnectionAsync(webSocketContext.WebSocket);
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }
    }
    
    private async Task HandleConnectionAsync(WebSocket wSocket)
    {
        Console.WriteLine("New client connected.");
        var buffer = new byte[1024 * 4];

        try
        {
            while (wSocket.State == WebSocketState.Open)
            {
                var result = await wSocket.ReceiveAsync(new ArraySegment<byte>(buffer), default);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Console.WriteLine("Client disconnected.");
                    await wSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", default);
                    break;
                }

                var messageJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                
                await ProcessMessageAsync(messageJson, wSocket);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    
    private async Task ProcessMessageAsync(string messageJson, WebSocket webSocket)
    {
        try
        {
            var baseMessage = JsonSerializer.Deserialize<MessageBase>(messageJson);
            if (baseMessage == null)
            {
                Console.WriteLine("Invalid message received.");
                return;
            }
            
            var response = await _messageHandler.ResolveAndInvokeAsync(baseMessage.MessageType, messageJson);
            
            var responseJson = System.Text.Json.JsonSerializer.Serialize(response);
            await webSocket.SendAsync(Encoding.UTF8.GetBytes(responseJson), WebSocketMessageType.Text, true, default);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing message: {ex.Message}");
        }
    }
}