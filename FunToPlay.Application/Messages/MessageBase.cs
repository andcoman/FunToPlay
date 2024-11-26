namespace FunToPlay.Application.Messages;
using Metadata;

public class MessageBase
{
    public Metadata Metadata { get; set; }
    public string MessageType { get; set; }
}