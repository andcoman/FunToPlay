using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace FunToPlay.Application.Session;

public class SessionTracker
{
    private readonly ConcurrentDictionary<Guid, ConcurrentBag<string>> _playerSessions = new();

    public ConcurrentDictionary<Guid, ConcurrentBag<string>> PlayerSessions => _playerSessions;

    public void Add(Guid playerId, string deviceId)
    {
        _playerSessions.AddOrUpdate(playerId,
            _ => new ConcurrentBag<string> { deviceId },
            (guid, bag) =>
            {
                bag.Add(deviceId);
                return bag;
            });
    }

    public void Remove(string playerId, string deviceId)
    {
        var playerIdToGuid = Guid.Parse(playerId);

        if (_playerSessions.TryGetValue(playerIdToGuid, out var bag))
        {
            var item = bag.FirstOrDefault(x => x == deviceId);
            if (item != default)
            {
                bag.TryTake(out item);
            }
        }
    }
}
