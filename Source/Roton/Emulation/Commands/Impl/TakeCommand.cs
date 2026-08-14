using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "TAKE")]
[Context(Context.Super, "TAKE")]
public sealed class TakeCommand(Lazy<IEngine> engine) : ICommand
{
    private IEngine Engine => engine.Value;

    public void Execute(IOopContext context)
    {
        context.Resume = Engine.ExecuteTransaction(context, true);
        Engine.Hud.UpdateStatus();
    }
}