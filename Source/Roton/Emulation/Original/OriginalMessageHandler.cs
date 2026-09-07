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
    IScroll scroll,
    IScrollContent scrollContent)
    : IMessageHandler
{
    public ScrollResult ExecuteMessage(ref OopContext context)
    {
        switch (scrollContent.LineCount)
        {
            case 1:
            {
                messenger.SetMessage(facts.LongMessageDuration, new Message(scrollContent.GetLine(0)));
                return default;
            }
            case > 1:
            {
                state.KeyVector = Vector.Idle;
                return scroll.ShowMessage(context.Name, false, 0);
            }
            default:
            {
                return default;
            }
        }
    }

    public string[] GetMessageLines() => 
        [state.Message];
}