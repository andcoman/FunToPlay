using Version = FunToPlay.Application.Messages.Version;

namespace FunToPlay.Application.Metadata;

public class Metadata
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required int Version { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}