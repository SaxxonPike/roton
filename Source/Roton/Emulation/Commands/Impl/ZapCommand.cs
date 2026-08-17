using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "ZAP")]
[Context(Context.Super, "ZAP")]
public sealed class ZapCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(IOopContext context)
    {
        Span<char> buffer = stackalloc char[256];

        Engine.Parser.ReadWord(context.Index, context);
        context.SearchIndex = 0;
        while (true)
        {

            var result = Engine.ExecuteLabel(context.Index, context, Engine.State.GetOopWord(buffer), "\r:");
            if (!result)
                break;
            Engine.Actors[context.SearchIndex].Code[context.SearchOffset + 1] = 0x27;
        }
    }
}