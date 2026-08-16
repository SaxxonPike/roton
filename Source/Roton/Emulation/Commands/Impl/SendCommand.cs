using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "SEND")]
[Context(Context.Super, "SEND")]
public sealed class SendCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(IOopContext context)
    {
        var target = Engine.Parser.ReadWord(context.Index, context);
        context.NextLine = Engine.BroadcastLabel(context.Index, target, false);
    }
}