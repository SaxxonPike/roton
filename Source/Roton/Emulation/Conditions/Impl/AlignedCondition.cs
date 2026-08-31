using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original, "ALLIGNED")]
[Context(Context.Super, "ALLIGNED")]
internal sealed class AlignedCondition(
    IActorList actors)
    : ICondition
{
    public bool? Execute(ref OopContext context, ref Word instruction)
    {
        return context.Actor.Location.X == actors.Player.Location.X ||
               context.Actor.Location.Y == actors.Player.Location.Y;
    }
}