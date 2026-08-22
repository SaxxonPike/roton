using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "RESTORE")]
[Context(Context.Super, "RESTORE")]
public sealed class RestoreCommand(
    IEngineAccessor engine,
    IParser parser,
    IActorList actorList,
    IState state)
    : ICommand
{
    private IEngine Engine => engine.Instance;

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
            var result = Engine.ExecuteLabel(context.Index, ref context.Search, state.GetOopWord(wordBuffer), "\r'");
            if (!result)
                break;

            while (context.Search.Offset >= 0)
            {
                actorList[context.Search.Index].Code.Span[context.Search.Offset + 1] = ':';
                var word = state.GetOopWord(wordBuffer);
                context.Search.Offset = parser.Search(context.Search.Index, buffer.Slice(0, word.Length + 2));
            }
        }
    }
}