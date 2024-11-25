using FunToPlay.Application.Messages;
using FunToPlay.Application.Messages.Responses;
using FunToPlay.Domain.Utils;

namespace FunToPlay.Application.Handlers;

public class SendGiftHandler : IHandler<SendGiftRequest, SendGiftMessageLoginResponse>
{
    public Task<OperationResult<SendGiftMessageLoginResponse>> HandleAsync(SendGiftRequest message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}