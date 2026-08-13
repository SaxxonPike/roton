using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original, "CONTACT")]
[Context(Context.Super, "CONTACT")]
public sealed class ContactCondition : ICondition
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public ContactCondition(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public bool? Execute(IOopContext context)
    {
        var player = Engine.Player;
        var distance = new Location16(context.Actor.Location).Difference(player.Location);
        return distance.X * distance.X + distance.Y * distance.Y == 1;
    }
}