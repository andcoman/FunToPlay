namespace FunToPlay.Application.Messages;

public class SendGiftRequest : MessageBase
{
    public string FriendPlayerId { get; set; }
    public string ResourceType { get; set; }
    public long ResourceValue { get; set; }
    public override Metadata.Metadata Metadata { get; set; }
    public override string MessageType { get; set; } = nameof(SendGiftRequest);
}