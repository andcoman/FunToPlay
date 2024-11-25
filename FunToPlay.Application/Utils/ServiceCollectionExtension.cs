using FunToPlay.Application.Handlers;
using FunToPlay.Application.Messages;
using FunToPlay.Application.Messages.Responses;
using Microsoft.Extensions.DependencyInjection;

namespace FunToPlay.Application.Utils;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<IHandler<LoginRequest, LoginMessageResponse>, LoginHandler>();
        services.AddScoped<IHandler<UpdateResourcesRequest, UpdateResourceMessageResponse>, UpdateResourcesHandler>();
        services.AddScoped<IHandler<SendGiftRequest, SendGiftMessageLoginResponse>, SendGiftHandler>();
        
        services.AddSingleton<MessageHandler>(serviceProvider =>
        {
            var resolver = new MessageHandler(serviceProvider);
            resolver.Register<LoginRequest, LoginMessageResponse, LoginHandler>(MessageRoutes.Login);
            resolver.Register<UpdateResourcesRequest, UpdateResourceMessageResponse, UpdateResourcesHandler>(MessageRoutes.UpdateResources);
            resolver.Register<SendGiftRequest, SendGiftMessageLoginResponse, SendGiftHandler>(MessageRoutes.SendGift);
            
            return resolver;
        });

        return services;
    }
}