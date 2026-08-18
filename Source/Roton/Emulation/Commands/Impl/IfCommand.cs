using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "IF")]
[Context(Context.Super, "IF")]
public sealed class IfCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        var condition = Engine.Parser.GetCondition(ref context, ref instruction);
            
        if (condition.HasValue)
            context.Resume = condition.Value;
    }
}