using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperObjectMover(IEngineAccessor engine) : IObjectMover
{
    private IEngine Engine => engine.Instance;

    public void ExecuteDirection(ref OopContext context, Vector vector)
    {
        var count = Engine.Parser.ReadNumber(context.Index, ref context.Actor.Instruction);
        if (count < 0)
            count = 1;

        if (context.Command == 0x3F) // ?
            count = -count;

        context.Actor.P2 = unchecked((byte)count);
        context.Actor.Vector = vector;
        context.Repeat = false;
    }
        
}