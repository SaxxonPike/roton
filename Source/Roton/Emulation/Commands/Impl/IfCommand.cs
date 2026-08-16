using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "IF")]
[Context(Context.Super, "IF")]
public sealed class IfCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(IOopContext context)
    {
        var condition = Engine.Parser.GetCondition(context);
            
        if (condition.HasValue)
            context.Resume = condition.Value;
    }
}