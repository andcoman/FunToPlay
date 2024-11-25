using FunToPlay.Domain.Entities;
using FunToPlay.Infrastructure.Data.Entities;

namespace FunToPlay.Infrastructure.Extensions;

public static class PlayerExtensions
{
    public static PlayerDbEntity ToDbEntity(this Player player)
    {
        return new PlayerDbEntity
        {
            PlayerId = player.PlayerId,
            DeviceId = player.DeviceId,
            IsConnected = player.IsConnected,
            Resources = player.Resources.Select(r => new ResourceDbEntity
            {
                ResourceId = System.Guid.NewGuid(),
                ResourceType = r.Type.ToString(),
                ResourceValue = r.Value
            }).ToList()
        };
    }
}