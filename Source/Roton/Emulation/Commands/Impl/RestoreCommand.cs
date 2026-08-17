using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "RESTORE")]
[Context(Context.Super, "RESTORE")]
public sealed class RestoreCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(IOopContext context)
    {
        Span<char> buffer = stackalloc char[250];
        buffer[0] = '\r';
        buffer[1] = '\'';
        
        Engine.Parser.ReadWord(context.Index, context);
        context.SearchIndex = 0;
        while (true)
        {
            var result = Engine.ExecuteLabel(context.Index, context, Engine.State.OopWord, "\r'");
            if (!result)
                break;

            while (context.SearchOffset >= 0)
            {
                Engine.Actors[context.SearchIndex].Code[context.SearchOffset + 1] = 0x3A;
                var word = Engine.State.OopWord.AsSpan();
                word.CopyTo(buffer.Slice(2));
                context.SearchOffset = Engine.Parser.Search(context.SearchIndex, buffer.Slice(0, word.Length + 2));
            }
        }
    }
}