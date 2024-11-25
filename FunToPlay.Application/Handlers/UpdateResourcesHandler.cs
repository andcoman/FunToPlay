using FunToPlay.Application.Messages;
using FunToPlay.Application.Messages.Responses;
using FunToPlay.Domain.Utils;
using Version = FunToPlay.Application.Messages.Version;

namespace FunToPlay.Application.Handlers;

public class UpdateResourcesHandler : IHandler<UpdateResourcesRequest, UpdateResourceMessageResponse>
{
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
        throw new NotImplementedException();
    }
}