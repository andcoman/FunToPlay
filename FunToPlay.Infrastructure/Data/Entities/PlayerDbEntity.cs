using FunToPlay.Infrastructure.Data.Entities;

namespace FunToPlay.Infrastructure.Data.Entities;

public class PlayerDbEntity
{
    public Guid PlayerId { get; set; }
    public string DeviceId { get; set; }
    public bool IsConnected { get; set; }
    public ICollection<ResourceDbEntity> Resources { get; set; }
}
