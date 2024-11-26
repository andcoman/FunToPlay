using FunToPlay.Domain.Entities;

namespace FunToPlay.Application.Messages;

public class UpdateResourcesRequest : MessageBase
{
    public Metadata.Metadata Metadata { get; set; }
    public string MessageType => "UpdateResources";
    public string ResourceType { get; set; }
    public long ResourceValue{ get; set; }
}