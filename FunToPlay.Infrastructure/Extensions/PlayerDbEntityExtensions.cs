using FunToPlay.Domain.Entities;
using FunToPlay.Domain.ValueObjects;
using FunToPlay.Infrastructure.Data.Entities;

namespace FunToPlay.Infrastructure.Extensions;

public static class PlayerDbEntityExtensions
{
    public static Player? ToDomainEntity(this PlayerDbEntity playerDb)
    {
        var player = new Player(playerDb.PlayerId, playerDb.DeviceId);

        foreach (var resourceDb in playerDb.Resources)
        {
            player.Resources.Add(new Resource(resourceDb.ResourceType, resourceDb.ResourceValue));
        }

        if (playerDb.IsConnected)
            player.Login();

        return player;
    }
}