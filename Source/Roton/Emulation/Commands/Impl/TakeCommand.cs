using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "TAKE")]
[Context(Context.Super, "TAKE")]
public sealed class TakeCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        context.Resume = Engine.ExecuteTransaction(ref context, ref instruction, true);
        Engine.Hud.UpdateStatus();
    }
}