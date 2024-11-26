using FluentValidation;
using FunToPlay.Application.Handlers;
using FunToPlay.Application.Messages;
using FunToPlay.Application.Messages.Responses;
using FunToPlay.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace FunToPlay.Application.Utils;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<LoginHandler>();
        services.AddScoped<UpdateResourcesHandler>();
        services.AddScoped<SendGiftHandler>();

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

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<LoginMessageRequestValidator>();

        return services;
    }
}