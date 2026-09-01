using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperPlayerEnterHandler(
    IBoardTime boardTime,
    IBroadcaster broadcaster,
    IFacts facts,
    IBoard board,
    IActorList actors,
    IHud hud,
    IWorld world)
    : IPlayerEnterHandler
{
    public void EnterBoard()
    {
        boardTime.Reset();
        broadcaster.BroadcastLabel(0, facts.EnterLabel, false);
        board.Entrance = actors.Player.Location;
        hud.UpdateCamera();
        world.TimePassed = 0;
        hud.UpdateStatus();
    }
}