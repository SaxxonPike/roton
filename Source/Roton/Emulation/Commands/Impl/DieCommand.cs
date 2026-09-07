using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "DIE")]
[Context(Context.Super, "DIE")]
internal sealed class DieCommand(
    IElementList elements)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        context.Died = true;
        context.DeathTile = new Tile(elements.EmptyId, 0);
    }
}