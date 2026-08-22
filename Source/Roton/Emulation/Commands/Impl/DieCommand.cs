using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "DIE")]
[Context(Context.Super, "DIE")]
public sealed class DieCommand(
    IEngineAccessor engine,
    IElementList elementList)
    : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        context.Died = true;
        context.DeathTile = new Tile(elementList.EmptyId, 0);
    }
}