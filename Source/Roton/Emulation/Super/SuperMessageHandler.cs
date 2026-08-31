using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public class SuperMessageHandler(
    IEngineAccessor engine,
    IHud hud,
    IFacts facts,
    IState state,
    IMessenger messenger)
    : IMessageHandler
{
    public IScrollState? ExecuteMessage(ref OopContext context)
    {
        if (!context.HasMessage)
            return null;

        var message = context.GetMessage();

        switch (message.Count)
        {
            case 1:
                messenger.SetMessage(facts.LongMessageDuration, new Message(string.Empty, message[0]));
                return null;
            case 2:
                messenger.SetMessage(facts.LongMessageDuration,
                    new Message(message[0], message[1]));
                return null;
            case 0:
                return null;
            default:
                state.KeyVector = Vector.Idle;
                return hud.ShowScroll(false, context.Name, [.. message]);
        }
    }
}