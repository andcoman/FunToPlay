using FunToPlay.Domain.Entities;

namespace FunToPlay.Domain.Repositories;

public interface IPlayerRepository
{
    Task<Player?> GetByIdAsync(Guid playerId, CancellationToken cancellationToken);
    Task<Player?> GetAsync(Player player, CancellationToken cancellationToken);
    Task<Player?> GetByDeviceIdAsync(string deviceId, CancellationToken cancellationToken);
    Task UpsertAsync(Player player, CancellationToken cancellationToken);
    Task DeleteAsync(Player? player, CancellationToken cancellationToken);
    Task DeleteByIdAsync(Guid playerId, CancellationToken cancellationToken);
}