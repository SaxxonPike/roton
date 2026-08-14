using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "END")]
[Context(Context.Super, "END")]
public sealed class EndCommand(Lazy<IEngine> engine) : ICommand
{
    private IEngine Engine => engine.Value;

    public void Execute(IOopContext context)
    {
        Engine.State.OopByte = 0;
        context.Instruction = -1;
    }
}