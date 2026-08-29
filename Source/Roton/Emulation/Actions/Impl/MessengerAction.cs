using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x02)]
[Context(Context.Super, 0x02)]
internal sealed class MessengerAction(
    IEngineAccessor engine,
    IActorList actorList,
    IHud hud,
    IFeatures features,
    IState state)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actorList[index];
        if (actor.Location.X == 0)
        {
            hud.DrawMessage(new Message(features.GetMessageLines()), actor.P2 % 7 + 9);
            actor.P2--;
            if (actor.P2 > 0) return;

            Engine.RemoveActor(index);
            state.ActIndex--;
            hud.UpdateBorder();
            state.Message = string.Empty;
            state.Message2 = string.Empty;
        }
    }
}