using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "CYCLE")]
[Context(Context.Super, "CYCLE")]
public sealed class CycleCommand(Lazy<IEngine> engine) : ICommand
{
    private IEngine Engine => engine.Value;

    public void Execute(IOopContext context)
    {
        var value = Engine.Parser.ReadNumber(context.Index, context);
        if (value > 0)
        {
            context.Actor.Cycle = value;
        }
    }
}