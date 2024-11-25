using FunToPlay.Domain.Entities;

namespace FunToPlay.Application.Messages;

public class UpdateResourcesRequest : MessageBase
{
    public override Metadata.Metadata Metadata { get; set; }
    public override string MessageType { get; set; }
    public string ResourceType { get; set; }
    public long ResourceValue{ get; set; }
}