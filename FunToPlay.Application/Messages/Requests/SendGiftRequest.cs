namespace FunToPlay.Application.Messages;

public class SendGiftRequest : MessageBase
{
    public string FriendPlayerId { get; set; }
    public string ResourceType { get; set; }
    public long ResourceValue { get; set; }
    public Metadata.Metadata Metadata { get; set; }
    public string MessageType => "SendGift";
}