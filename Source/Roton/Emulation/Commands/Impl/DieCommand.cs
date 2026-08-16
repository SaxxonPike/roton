using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "DIE")]
[Context(Context.Super, "DIE")]
public sealed class DieCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(IOopContext context)
    {
        context.Died = true;
        context.DeathTile.SetTo(Engine.ElementList.EmptyId, 0);
    }
}