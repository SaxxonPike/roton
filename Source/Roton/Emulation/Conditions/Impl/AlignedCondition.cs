using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original, "ALLIGNED")]
[Context(Context.Super, "ALLIGNED")]
public sealed class AlignedCondition(IEngineAccessor engine) : ICondition
{
    private IEngine Engine => engine.Instance;

    public bool? Execute(IOopContext context)
    {
        return context.Actor.Location.X == Engine.Player.Location.X ||
               context.Actor.Location.Y == Engine.Player.Location.Y;
    }
}