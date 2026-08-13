using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "ENDGAME")]
[Context(Context.Super, "ENDGAME")]
public sealed class EndgameCommand : ICommand
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public EndgameCommand(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public void Execute(IOopContext context)
    {
        Engine.World.Health = 0;
    }
}