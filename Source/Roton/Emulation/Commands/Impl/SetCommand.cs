using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "SET")]
[Context(Context.Super, "SET")]
public sealed class SetCommand(Lazy<IEngine> engine) : ICommand
{
    private IEngine Engine => engine.Value;

    public void Execute(IOopContext context)
    {
        var flag = Engine.Parser.ReadWord(context.Index, context);
        Engine.World.Flags.Add(flag);
    }
}