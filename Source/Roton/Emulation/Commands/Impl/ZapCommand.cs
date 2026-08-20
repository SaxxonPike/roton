using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "ZAP")]
[Context(Context.Super, "ZAP")]
public sealed class ZapCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];

        Engine.Parser.ReadWord(context.Index, ref instruction);
        context.Search.Index = 0;

        while (true)
        {
            var result = Engine.ExecuteLabel(context.Index, ref context.Search, Engine.State.GetOopWord(buffer), "\r:");

            if (!result)
                break;

            Engine.Actors[context.Search.Index].Code.Span[context.Search.Offset + 1] = '\'';
        }
    }
}