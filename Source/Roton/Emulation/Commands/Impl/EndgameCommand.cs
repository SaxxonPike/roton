using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "ENDGAME")]
[Context(Context.Super, "ENDGAME")]
public sealed class EndgameCommand(
    IEngineAccessor engine,
    IWorld world) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        world.Health = 0;
    }
}