using System.Net.WebSockets;
using System.Xml;
using FluentValidation;
using FunToPlay.Application.Messages;
using FunToPlay.Application.Messages.Responses;
using FunToPlay.Application.Session;
using FunToPlay.Domain.Repositories;
using FunToPlay.Domain.Utils;
using Version = FunToPlay.Application.Messages.Version;

namespace FunToPlay.Application.Handlers;

public class UpdateResourcesHandler : IHandler<UpdateResourcesRequest, UpdateResourceMessageResponse>
{
    private readonly SessionTracker _sessionTracker;
    private readonly IValidator<UpdateResourcesRequest> _validator;
    private readonly IPlayerRepository _playerRepository;

    public UpdateResourcesHandler(SessionTracker sessionTracker, IValidator<UpdateResourcesRequest> validator, IPlayerRepository playerRepository)
    {
        _sessionTracker = sessionTracker;
        _validator = validator;
        _playerRepository = playerRepository;
    }

    public async Task<OperationResult<UpdateResourceMessageResponse>> HandleAsync(UpdateResourcesRequest message, CancellationToken cancellationToken = default)
    {
        try
        {
            if (message.Metadata.Version == Version.V1)
            {
                return await HandleInternalV1Async(message);
            }
            
            return OperationResult<UpdateResourceMessageResponse>.Failure("Unsupported version");
        }
        catch (Exception e)
        {
            return OperationResult<UpdateResourceMessageResponse>.Failure(e.Message);
        }
    }

    private async Task<OperationResult<UpdateResourceMessageResponse>> HandleInternalV1Async(UpdateResourcesRequest message)
    {
        var validationResult = await _validator.ValidateAsync(message);
        if (!validationResult.IsValid)
        {
            return OperationResult<UpdateResourceMessageResponse>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToString());
        }

        var playerId = _sessionTracker.GetPlayerIdByWebSocketHashCode(message.Metadata.WebSocketHashCode);

        await _playerRepository.AddOrUpdateResourceAsync(playerId.Value, message.ResourceType, message.ResourceValue,
            default);

        var balance = await _playerRepository.GetResourcesAsync(playerId.Value, default);

        var result = new UpdateResourceMessageResponse
        {
            NewBalance = balance.Values.Sum().ToString()
        };

        return OperationResult<UpdateResourceMessageResponse>.Success(result);
    }
}