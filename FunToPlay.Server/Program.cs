// See https://aka.ms/new-console-template for more information

using FluentValidation;
using FunToPlay.Application.Session;
using FunToPlay.Application.Utils;
using FunToPlay.Domain.Repositories;
using FunToPlay.Server.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Started");

using var host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddHandlers();
            services.AddValidators();
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<SessionTracker>();
            services.AddSingleton<WebSocketService>();
        })
        .Build();
    
var webSocketServer = host.Services.GetRequiredService<WebSocketService>();
await webSocketServer.StartAsync("http://localhost:5000/");

Console.WriteLine("WebSocket server is running. Press Ctrl+C to shut down.");
await host.WaitForShutdownAsync();
