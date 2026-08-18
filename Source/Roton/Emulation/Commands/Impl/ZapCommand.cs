using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "ZAP")]
[Context(Context.Super, "ZAP")]
public sealed class ZapCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[256];

        Engine.Parser.ReadWord(context.Index, ref instruction);
        context.Search.Index = 0;
        while (true)
        {

            var result = Engine.ExecuteLabel(context.Index, ref context.Search, Engine.State.GetOopWord(buffer), "\r:");
            if (!result)
                break;
            Engine.Actors[context.Search.Index].Code[context.Search.Offset + 1] = 0x27;
        }
    }
}