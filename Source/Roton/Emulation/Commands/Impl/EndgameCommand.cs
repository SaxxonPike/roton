using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "ENDGAME")]
[Context(Context.Super, "ENDGAME")]
public sealed class EndgameCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        Engine.World.Health = 0;
    }
}