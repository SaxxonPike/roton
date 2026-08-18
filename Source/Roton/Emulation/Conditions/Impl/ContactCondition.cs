using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original, "CONTACT")]
[Context(Context.Super, "CONTACT")]
public sealed class ContactCondition(IEngineAccessor engine) : ICondition
{
    private IEngine Engine => engine.Instance;

    public bool? Execute(IOopContext context)
    {
        var player = Engine.Player;
        var selfLoc = context.Actor.Location;
        var playerLoc = player.Location;
        var distance = new Location16(selfLoc.X, selfLoc.Y) - new Location16(playerLoc.X, playerLoc.Y);
        return distance.X * distance.X + distance.Y * distance.Y == 1;
    }
}