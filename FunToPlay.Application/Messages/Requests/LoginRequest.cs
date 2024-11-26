using FunToPlay.Application.Handlers;

namespace FunToPlay.Application.Messages;

public class LoginRequest : MessageBase
{
    public  Metadata.Metadata Metadata { get; set; }
    public string MessageType => "Login";
    public string DeviceId { get; set; }
}