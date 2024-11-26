using System.Data.SQLite;
using FunToPlay.Domain.Entities;
using FunToPlay.Domain.Repositories;
using FunToPlay.Infrastructure.Data.Entities;
using FunToPlay.Infrastructure.Extensions;

public class PlayerRepository : IPlayerRepository
{
    private static readonly string basePath = new DirectoryInfo(AppContext.BaseDirectory)
        .Parent?.Parent?.Parent?.Parent?.FullName ?? throw new InvalidOperationException("Base path could not be determined.");

    private readonly string _connectionString = $"Data Source={Path.Combine(basePath, "FunToPlay.sqlite")};";

    public async Task<Player?> GetByIdAsync(Guid playerId, CancellationToken cancellationToken)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var command = new SQLiteCommand("SELECT * FROM Players WHERE PlayerId = @PlayerId", connection);
        command.Parameters.AddWithValue("@PlayerId", playerId.ToString());

        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (await reader.ReadAsync(cancellationToken))
        {
            var playerDb = new PlayerDbEntity
            {
                PlayerId = Guid.Parse(reader.GetString(0)),
                DeviceId = reader.GetString(1),
                IsConnected = reader.GetInt32(2) == 1,
            };
            return playerDb.ToDomainEntity();
        }

        return null;
    }

    public async Task<Player?> GetAsync(Player player, CancellationToken cancellationToken)
    {
        return await GetByIdAsync(player.PlayerId, cancellationToken);
    }

    public async Task<Player?> GetByDeviceIdAsync(string deviceId, CancellationToken cancellationToken)
    {
        await using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var command = new SQLiteCommand("SELECT * FROM Players WHERE DeviceId = @DeviceId", connection);
        command.Parameters.AddWithValue("@DeviceId", deviceId);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken)) 
            return null;
        
        var playerDb = new PlayerDbEntity
        {
            PlayerId = Guid.Parse(reader.GetString(0)),
            DeviceId = reader.GetString(1),
            IsConnected = reader.GetInt32(2) == 1,
        };
        return playerDb.ToDomainEntity();

    }

    public async Task UpsertAsync(Player player, CancellationToken cancellationToken)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var command = new SQLiteCommand(@"
                INSERT OR REPLACE INTO Players (PlayerId, DeviceId, IsConnected, CreatedAt)
                VALUES (@PlayerId, @DeviceId, @IsConnected, @CreatedAt);", connection);

        var playerDb = player.ToDbEntity();
        command.Parameters.AddWithValue("@PlayerId", playerDb.PlayerId);
        command.Parameters.AddWithValue("@DeviceId", playerDb.DeviceId);
        command.Parameters.AddWithValue("@IsConnected", playerDb.IsConnected);
        command.Parameters.AddWithValue("@IsConnected", playerDb.IsConnected);
        command.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);
        
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task DeleteAsync(Player player, CancellationToken cancellationToken)
    {
        await DeleteByIdAsync(player.PlayerId, cancellationToken);
    }

    public async Task DeleteByIdAsync(Guid playerId, CancellationToken cancellationToken)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var command = new SQLiteCommand("DELETE FROM Players WHERE PlayerId = @PlayerId", connection);
        command.Parameters.AddWithValue("@PlayerId", playerId.ToString());

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task AddOrUpdateResourceAsync(Guid playerId, string resourceType, long resourceValue, CancellationToken cancellationToken)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var command = new SQLiteCommand(@"
        INSERT OR REPLACE INTO PlayerResources (PlayerId, ResourceType, ResourceValue, CreatedAt)
        VALUES (@PlayerId, @ResourceType, @ResourceValue, @CreatedAt);", connection);

        command.Parameters.AddWithValue("@PlayerId", playerId.ToString());
        command.Parameters.AddWithValue("@ResourceType", resourceType);
        command.Parameters.AddWithValue("@ResourceValue", resourceValue);
        command.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<Dictionary<string, int>> GetResourcesAsync(Guid playerId, CancellationToken cancellationToken)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var command = new SQLiteCommand("SELECT ResourceType, ResourceValue FROM PlayerResources WHERE PlayerId = @PlayerId", connection);
        command.Parameters.AddWithValue("@PlayerId", playerId.ToString());

        using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var resources = new Dictionary<string, int>();
        while (await reader.ReadAsync(cancellationToken))
        {
            var resourceType = reader.GetString(0);
            var resourceValue = reader.GetInt32(1);
            resources[resourceType] = resourceValue;
        }

        return resources;
    }

    public async Task<int?> GetResourceValueAsync(Guid playerId, string resourceType, CancellationToken cancellationToken)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var command = new SQLiteCommand(@"
        SELECT ResourceValue 
        FROM PlayerResources 
        WHERE PlayerId = @PlayerId AND ResourceType = @ResourceType", connection);

        command.Parameters.AddWithValue("@PlayerId", playerId.ToString());
        command.Parameters.AddWithValue("@ResourceType", resourceType);

        using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (await reader.ReadAsync(cancellationToken))
        {
            return reader.GetInt32(0);
        }

        return null;
    }
}