using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original, "CONTACT")]
[Context(Context.Super, "CONTACT")]
public sealed class ContactCondition(Lazy<IEngine> engine) : ICondition
{
    private IEngine Engine => engine.Value;

    public bool? Execute(IOopContext context)
    {
        var player = Engine.Player;
        var distance = new Location16(context.Actor.Location).Difference(player.Location);
        return distance.X * distance.X + distance.Y * distance.Y == 1;
    }
}