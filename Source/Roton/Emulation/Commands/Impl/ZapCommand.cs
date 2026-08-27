using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "ZAP")]
[Context(Context.Super, "ZAP")]
public sealed class ZapCommand(
    IParser parser,
    IState state,
    IActorList actorList,
    IBroadcaster broadcaster)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];

        parser.ReadWord(context.Index, ref instruction);
        context.Search.Index = 0;

        while (true)
        {
            var result = broadcaster.ExecuteLabel(context.Index, ref context.Search, state.GetOopWord(buffer), "\r:");

            if (!result)
                break;

            actorList[context.Search.Index].Code[context.Search.Offset + 1] = '\'';
        }
    }
}