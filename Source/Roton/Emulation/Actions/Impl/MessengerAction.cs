using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the messenger element.
/// </summary>
[Context(Context.Original, 0x02)]
[Context(Context.Super, 0x02)]
internal sealed class MessengerAction(
    IActorList actors,
    IHud hud,
    IState state,
    IMessageHandler messageHandler,
    IActorManager actorManager)
    : IAction
{
    public void Act(int index)
    {
        var actor = actors[index];
        if (actor.Location.X == 0)
        {
            hud.DrawMessage(new Message(messageHandler.GetMessageLines()), actor.P2 % 7 + 9);

            actor.P2--;
            if (actor.P2 > 0)
                return;

            actorManager.Free(index);
            state.ActIndex--;
            hud.UpdateBorder();
            state.Message = string.Empty;
            state.Message2 = string.Empty;
        }
    }
}