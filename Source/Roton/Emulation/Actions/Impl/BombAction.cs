using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x0D)]
[Context(Context.Super, 0x0D)]
public sealed class BombAction(
    IEngineAccessor engine,
    ISounds sounds,
    IActorList actorList,
    ISoundUnit soundUnit,
    IBoardUpdater boardUpdater,
    IRadiusUpdater radiusUpdater)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actorList[index];
        if (actor.P1 <= 0)
            return;

        actor.P1--;
        boardUpdater.UpdateBoard(actor.Location);
        switch ((int)actor.P1)
        {
            case 1:
                soundUnit.PlaySound(1, sounds.BombExplode);
                radiusUpdater.UpdateRadius(actor.Location, RadiusMode.Explode);
                break;
            case 0:
                var location = actor.Location;
                Engine.RemoveActor(index);
                radiusUpdater.UpdateRadius(location, RadiusMode.Clear);
                break;
            default:
                soundUnit.PlaySound(1, (actor.P1 & 0x01) == 0 ? sounds.BombTock : sounds.BombTick);
                break;
        }
    }
}