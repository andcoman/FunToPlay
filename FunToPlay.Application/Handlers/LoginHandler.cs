using System.Net.WebSockets;
using FluentValidation;
using FunToPlay.Application.Messages;
using FunToPlay.Application.Messages.Responses;
using FunToPlay.Application.Session;
using FunToPlay.Domain.Entities;
using FunToPlay.Domain.Repositories;
using FunToPlay.Domain.Services;
using FunToPlay.Domain.Utils;
using Version = FunToPlay.Application.Messages.Version;

namespace FunToPlay.Application.Handlers;

public class LoginHandler : IHandler<LoginRequest, LoginMessageResponse>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IValidator<LoginRequest> _validator;
    private readonly SessionTracker _sessionTracker;
    
    public LoginHandler(IPlayerRepository playerRepository, IValidator<LoginRequest> validator,
        SessionTracker sessionTracker)
    {
        _playerRepository = playerRepository;
        _validator = validator;
        _sessionTracker = sessionTracker;
    }
    
    public async Task<OperationResult<LoginMessageResponse>> HandleAsync(LoginRequest message, CancellationToken cancellationToken = default)
    {
        try
        {
            if (message.Metadata.Version == Version.V1)
            {
                return await HandleInternalV1Async(message);
            }
            
            return OperationResult<LoginMessageResponse>.Failure("Unsupported version");
        }
        catch (Exception e)
        {
            return OperationResult<LoginMessageResponse>.Failure(e.Message);
        }
    }
    
    private async Task<OperationResult<LoginMessageResponse>> HandleInternalV1Async(LoginRequest message)
    {
        var validationResult = await _validator.ValidateAsync(message);
        if (!validationResult.IsValid)
        {
            return OperationResult<LoginMessageResponse>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToString());
        }
        
        var player = await _playerRepository.GetByDeviceIdAsync(message.DeviceId, CancellationToken.None);
        
        if (player == null)
        {
            player = new Player(Guid.NewGuid(), message.DeviceId);
            await _playerRepository.UpsertAsync(player, CancellationToken.None);
        }
        
        if (player.IsConnected)
        {
            return OperationResult<LoginMessageResponse>.Failure("Player is already logged in.");
        }
        
        await HandlePlayerLoginAsync(player);
        
        var response = new LoginMessageResponse
        {
            PlayerId = player.PlayerId.ToString(),
        };

        return OperationResult<LoginMessageResponse>.Success(response);
    }
    
    private async Task HandlePlayerLoginAsync(Player player)
    {
        try
        {
            player.Login();
            await _playerRepository.UpsertAsync(player, CancellationToken.None);
            _sessionTracker.Add(player.PlayerId, player.DeviceId);
        }
        catch (Exception)
        {
            player.Logout();
            await _playerRepository.UpsertAsync(player, CancellationToken.None);
            throw;
        }
    }
}
