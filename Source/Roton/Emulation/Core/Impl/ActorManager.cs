using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class ActorManager(
    ITracer tracer,
    IActorList actors,
    IState state,
    ITiles tiles,
    IBoardUpdater boardUpdater,
    ICodeHeap heap) 
    : IActorManager
{
    public void Free(int index)
    {
        if (index < 0)
        {
            tracer.TraceCrash("Attempted to remove invalid actor index");
            return;
        }

        var actor = actors[index];
        var freeCode = actor.Length > 0 && actor.Pointer != 0;

        if (index < state.ActIndex)
            state.ActIndex--;

        tiles[actor.Location] = actor.UnderTile;

        if (actor.Location.Y > 0)
            boardUpdater.UpdateBoard(actor.Location);

        var pointer = actor.Pointer;

        for (var i = 1; i <= state.ActorCount; i++)
        {
            var a = actors[i];
            if (a.Follower >= index)
            {
                if (a.Follower == index)
                    a.Follower = -1;
                else
                    a.Follower--;
            }

            if (a.Leader >= index)
            {
                if (a.Leader == index)
                    a.Leader = -1;
                else
                    a.Leader--;
            }

            if (freeCode && i != index && a.Pointer == pointer)
                freeCode = false;
        }

        if (freeCode)
        {
            heap.Free(pointer);
            actor.Pointer = 0;
        }

        if (index < state.ActorCount)
            for (var i = index; i < state.ActorCount; i++)
                actors[i].CopyFromByRaw(actors[i + 1]);

        state.ActorCount--;
    }

}