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
    IScroll scroll,
    IScrollContent scrollContent)
    : IMessageHandler
{
    public ScrollResult ExecuteMessage(ref OopContext context)
    {
        if (scrollContent.LineCount <= 0)
            return default;

        switch (scrollContent.LineCount)
        {
            case 1:
                messenger.SetMessage(facts.LongMessageDuration, new Message(string.Empty, scrollContent.GetLine(0)));
                return default;
            case 2:
                messenger.SetMessage(facts.LongMessageDuration, new Message(scrollContent.GetLine(0), scrollContent.GetLine(1)));
                return default;
            case 0:
                return default;
            default:
                state.KeyVector = Vector.Idle;
                return scroll.ShowMessage(context.Name, false, 0);
        }
    }

    public string[] GetMessageLines()
    {
        return string.IsNullOrEmpty(state.Message2)
            ? [string.Empty, state.Message]
            : [state.Message, state.Message2];
    }
}