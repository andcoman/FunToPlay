using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace FunToPlay.Application.Session;

public class SessionTracker
{
    private readonly ConcurrentDictionary<Guid, WebSocketTracker> _playerSessions = new();

    public ConcurrentDictionary<Guid, WebSocketTracker> PlayerSessions => _playerSessions;

    public void Add(Guid playerId, string deviceId ,WebSocket webSocket)
    {
        _playerSessions.AddOrUpdate(playerId,
            new WebSocketTracker
            {
                DeviceId = deviceId,
                WebSocket = webSocket
            },
            (key, existingTracker) =>
            {
                existingTracker.WebSocket = webSocket;
                return existingTracker;
            });
    }

    public Guid? GetPlayerIdByWebSocketHashCode(int webSocketHashCode)
    {
        foreach (var kvp in _playerSessions)
        {
            if (kvp.Value.WebSocket.GetHashCode() == webSocketHashCode)
            {
                return kvp.Key;
            }
        }

        return null;
    }
}
