using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "TAKE")]
[Context(Context.Super, "TAKE")]
public sealed class TakeCommand : ICommand
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public TakeCommand(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public void Execute(IOopContext context)
    {
        context.Resume = Engine.ExecuteTransaction(context, true);
        Engine.Hud.UpdateStatus();
    }
}