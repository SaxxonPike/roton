using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "ZAP")]
[Context(Context.Super, "ZAP")]
public sealed class ZapCommand(Lazy<IEngine> engine) : ICommand
{
    private IEngine Engine => engine.Value;

    public void Execute(IOopContext context)
    {
        Engine.Parser.ReadWord(context.Index, context);
        context.SearchIndex = 0;
        while (true)
        {
            
            var result = Engine.ExecuteLabel(context.Index, context, Engine.State.OopWord,"\xD\x3A");
            if (!result)
                break;
            Engine.Actors[context.SearchIndex].Code[context.SearchOffset + 1] = 0x27;
        }
    }
}