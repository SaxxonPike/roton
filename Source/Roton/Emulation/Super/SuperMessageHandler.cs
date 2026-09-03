using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperMessageHandler(
    IFacts facts,
    IState state,
    IMessenger messenger,
    IScroll scroll)
    : IMessageHandler
{
    public ScrollResult ExecuteMessage(ref OopContext context)
    {
        if (!context.HasMessage)
            return default;

        var message = context.GetMessage();

        switch (message.Count)
        {
            case 1:
                messenger.SetMessage(facts.LongMessageDuration, new Message(string.Empty, message[0]));
                return default;
            case 2:
                messenger.SetMessage(facts.LongMessageDuration,
                    new Message(message[0], message[1]));
                return default;
            case 0:
                return default;
            default:
                state.KeyVector = Vector.Idle;
                return scroll.ShowMessage(context.Name, [.. message], false, 0);
        }
    }

    public string[] GetMessageLines()
    {
        return string.IsNullOrEmpty(state.Message2)
            ? [string.Empty, state.Message]
            : [state.Message, state.Message2];
    }
}