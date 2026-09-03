using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "RESTORE")]
[Context(Context.Super, "RESTORE")]
internal sealed class RestoreCommand(
    IParser parser,
    IActorList actors,
    IState state,
    IBroadcaster broadcaster)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        buffer[0] = '\r';
        buffer[1] = '\'';
        var wordBuffer = buffer.Slice(2);

        parser.ReadWord(context.Index, ref instruction);
        context.Search.Index = 0;

        while (true)
        {
            var result =
                broadcaster.ExecuteLabel(context.Index, ref context.Search, state.GetOopWord(wordBuffer), "\r'");
            if (!result)
                break;

            while (context.Search.Offset >= 0)
            {
                actors[context.Search.Index].Code[context.Search.Offset + 1] = ':';
                var word = state.GetOopWord(wordBuffer);
                context.Search.Offset = parser.Search(context.Search.Index, buffer.Slice(0, word.Length + 2));
            }
        }
    }
}