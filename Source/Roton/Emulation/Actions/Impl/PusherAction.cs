using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x28)]
[Context(Context.Super, 0x28)]
public sealed class PusherAction(IEngineAccessor engine) : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        var source = actor.Location;

        if (!Engine.Tiles.ElementAt(actor.Location + actor.Vector).IsFloor)
        {
            Engine.Push(actor.Location + actor.Vector, actor.Vector);
        }

        index = Engine.Actors.ActorIndexAt(source);
        actor = Engine.Actors[index];
            
        if (!Engine.Tiles.ElementAt(actor.Location + actor.Vector).IsFloor) 
            return;

        Engine.MoveActor(index, actor.Location + actor.Vector);
        Engine.PlaySound(2, Engine.Sounds.Push);
        var behindLocation = actor.Location - actor.Vector;
            
        if (Engine.Tiles[behindLocation].Id != Engine.ElementList.PusherId) 
            return;

        var behindIndex = Engine.Actors.ActorIndexAt(behindLocation);
        var behindActor = Engine.Actors[behindIndex];
        if (behindActor.Vector.X == actor.Vector.X && behindActor.Vector.Y == actor.Vector.Y)
        {
            Engine.ActionList.Get(Engine.ElementList.PusherId).Act(behindIndex);
        }
    }
}