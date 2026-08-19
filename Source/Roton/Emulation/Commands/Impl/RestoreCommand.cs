using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "RESTORE")]
[Context(Context.Super, "RESTORE")]
public sealed class RestoreCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        buffer[0] = '\r';
        buffer[1] = '\'';
        var wordBuffer = buffer.Slice(2);
        
        Engine.Parser.ReadWord(context.Index, ref instruction);
        context.Search.Index = 0;
        while (true)
        {
            var result = Engine.ExecuteLabel(context.Index, ref context.Search, Engine.State.GetOopWord(wordBuffer), "\r'");
            if (!result)
                break;

            while (context.Search.Offset >= 0)
            {
                Engine.Actors[context.Search.Index].Code[context.Search.Offset + 1] = 0x3A;
                var word = Engine.State.GetOopWord(wordBuffer);
                context.Search.Offset = Engine.Parser.Search(context.Search.Index, buffer.Slice(0, word.Length + 2));
            }
        }
    }
}