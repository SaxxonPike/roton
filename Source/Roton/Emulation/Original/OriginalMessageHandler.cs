using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public class OriginalMessageHandler(
    IHud hud,
    IFacts facts,
    IState state,
    IMessenger messenger)
    : IMessageHandler
{
    public IScrollState? ExecuteMessage(ref OopContext context)
    {
        var message = context.GetMessage();

        switch (message)
        {
            case { Count: 1 }:
                messenger.SetMessage(facts.LongMessageDuration, new Message(message));
                return null;
            case { Count: > 1 }:
                state.KeyVector = Vector.Idle;
                return hud.ShowScroll(false, context.Name, [.. message]);
            default:
                return null;
        }
    }
}