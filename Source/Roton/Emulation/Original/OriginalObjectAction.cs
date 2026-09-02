using Roton.Emulation.Actions;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original, 0x24)]
internal sealed class OriginalObjectAction(
    IEngineAccessor engine,
    IActorList actors,
    ITiles tiles,
    IFacts facts,
    IBroadcaster broadcaster,
    IMover mover,
    ICodeExecutor codeExecutor)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actors[index];
        if (actor.Instruction >= 0)
        {
            codeExecutor.ExecuteCode(index, ref actor.Instruction, "Interaction");
        }

        if (actor.Vector.IsZero())
            return;

        var target = actor.Location + actor.Vector;
        if (tiles.ElementAt(target).IsFloor)
        {
            mover.MoveActor(index, target);
        }
        else
        {
            broadcaster.BroadcastLabel(-index, facts.ThudLabel, false);
        }
    }
}