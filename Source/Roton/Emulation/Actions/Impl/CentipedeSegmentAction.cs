using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x2D)]
[Context(Context.Super, 0x2D)]
public sealed class CentipedeSegmentAction(IEngineAccessor engine) : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        if (actor.Leader < 0)
        {
            if (actor.Leader < -1)
            {
                Engine.Tiles[actor.Location].Id = Engine.ElementList.HeadId;
            }
            else
            {
                actor.Leader--;
            }
        }
    }
}