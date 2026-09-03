using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalMessageHandler(
    IFacts facts,
    IState state,
    IMessenger messenger,
    IScroll scroll)
    : IMessageHandler
{
    public ScrollResult ExecuteMessage(ref OopContext context)
    {
        var message = context.GetMessage();

        switch (message)
        {
            case { Count: 1 }:
                messenger.SetMessage(facts.LongMessageDuration, new Message(message));
                return default;
            case { Count: > 1 }:
                state.KeyVector = Vector.Idle;
                return scroll.ShowMessage(context.Name, [.. message], false, 0);
            default:
                return default;
        }
    }

    public string[] GetMessageLines() => 
        [state.Message];
}