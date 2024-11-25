// See https://aka.ms/new-console-template for more information

using FunToPlay.Application.Handlers;
using FunToPlay.Application.Utils;
using FunToPlay.Domain.Repositories;
using FunToPlay.Server.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

static async Task Main(string[] args)
{
    using var host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddHandlers();
            services.AddSingleton<IPlayerRepository, PlayerRepository>();
            services.AddSingleton<WebSocketService>();
        })
        .Build();

    // Resolve the WebSocket server and start it
    var webSocketServer = host.Services.GetRequiredService<WebSocketService>();
    await webSocketServer.StartAsync("http://localhost:5000/");

    // Keep the console app alive
    Console.WriteLine("WebSocket server is running. Press Ctrl+C to shut down.");
    await host.WaitForShutdownAsync();
}