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
    Task AddOrUpdateResourceAsync(Guid playerId, string resourceType, long resourceValue, CancellationToken cancellationToken);
    Task<Dictionary<string, int>> GetResourcesAsync(Guid playerId, CancellationToken cancellationToken);
    Task<int?> GetResourceValueAsync(Guid playerId, string resourceType, CancellationToken cancellationToken);
}