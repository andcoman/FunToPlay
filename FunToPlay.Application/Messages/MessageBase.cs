namespace FunToPlay.Application.Messages;
using Metadata;

public abstract class MessageBase
{
    public abstract Metadata Metadata { get; set; }
    public abstract string MessageType { get; set; }
}