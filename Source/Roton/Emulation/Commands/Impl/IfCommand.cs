using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "IF")]
[Context(Context.Super, "IF")]
public sealed class IfCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        if (Engine.Parser.TryEvalCondition(ref context, ref instruction, out var result))
            context.Resume = result;
    }
}