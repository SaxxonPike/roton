using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "SEND")]
[Context(Context.Super, "SEND")]
public sealed class SendCommand(
    IEngineAccessor engine,
    IParser parser,
    IBroadcaster broadcaster)
    : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        var target = parser.ReadWord(context.Index, ref instruction, buffer);
        context.NextLine = broadcaster.BroadcastLabel(context.Index, target, false);
    }
}