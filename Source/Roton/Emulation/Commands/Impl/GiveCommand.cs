using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "GIVE")]
[Context(Context.Super, "GIVE")]
public sealed class GiveCommand(
    IHud hud,
    IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        context.Resume = Engine.ExecuteTransaction(ref context, ref instruction, false);
        hud.UpdateStatus();
    }
}