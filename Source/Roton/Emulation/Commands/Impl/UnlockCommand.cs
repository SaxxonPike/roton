using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "UNLOCK")]
[Context(Context.Super, "UNLOCK")]
public sealed class UnlockCommand(Lazy<IEngine> engine) : ICommand
{
    private IEngine Engine => engine.Value;

    public void Execute(IOopContext context)
    {
        Engine.UnlockActor(context.Index);
    }
}