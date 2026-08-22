using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original, "CONTACT")]
[Context(Context.Super, "CONTACT")]
public sealed class ContactCondition(
    IEngineAccessor engine,
    IActorList actorList)
    : ICondition
{
    private IEngine Engine => engine.Instance;

    public bool? Execute(ref OopContext context, ref Word instruction)
    {
        var player = actorList.Player;
        var selfLoc = context.Actor.Location;
        var playerLoc = player.Location;
        var distance = new Location16(selfLoc.X, selfLoc.Y) - new Location16(playerLoc.X, playerLoc.Y);
        return distance.X * distance.X + distance.Y * distance.Y == 1;
    }
}