using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "LOCK")]
[Context(Context.Super, "LOCK")]
public sealed class LockCommand(
    IEngineAccessor engine)
    : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        Engine.LockActor(context.Index);
    }
}