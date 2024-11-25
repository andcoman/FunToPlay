namespace FunToPlay.Application.Messages;

public class LoginRequest : MessageBase
{
    public override required Metadata.Metadata Metadata { get; set; }
    public override string MessageType { get; set; } = nameof(LoginRequest);
    public string DeviceId { get; set; }
}