using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalPlayerEnterHandler(
    IBoardTime boardTime,
    IBoard board,
    IActorList actors,
    IAlerts alerts,
    IMessenger messenger,
    IFacts facts,
    IWorld world,
    IHud hud)
    : IPlayerEnterHandler
{
    public void EnterBoard()
    {
        boardTime.Reset();
        board.Entrance = actors.Player.Location;
        if (board.IsDark && alerts.Dark)
        {
            messenger.SetMessage(facts.LongMessageDuration, alerts.DarkMessage);
            alerts.Dark = false;
        }

        world.TimePassed = 0;
        hud.UpdateStatus();
    }
}