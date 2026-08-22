using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "TAKE")]
[Context(Context.Super, "TAKE")]
public sealed class TakeCommand(
    IEngineAccessor engine,
    IHud hud)
    : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        context.Resume = Engine.ExecuteTransaction(ref context, ref instruction, true);
        hud.UpdateStatus();
    }
}