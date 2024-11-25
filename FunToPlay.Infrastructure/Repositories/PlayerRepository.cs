using System.Data.SQLite;
using FunToPlay.Domain.Entities;
using FunToPlay.Domain.Repositories;
using FunToPlay.Infrastructure.Data.Entities;
using FunToPlay.Infrastructure.Extensions;

public class PlayerRepository : IPlayerRepository
{
    private readonly string _connectionString = "$\"Data Source={Path.Combine(AppContext.BaseDirectory, \"FunToPlay.sqlite\")};\";";
    
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
            INSERT INTO Players (PlayerId, DeviceId, IsConnected, CreatedAt)
            VALUES (@PlayerId, @DeviceId, @IsConnected, @CreatedAt)
            ON CONFLICT(PlayerId) DO UPDATE SET
            DeviceId = excluded.DeviceId,
            IsConnected = excluded.IsConnected,
            CreatedAt = excluded.CreatedAt", connection);

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
}