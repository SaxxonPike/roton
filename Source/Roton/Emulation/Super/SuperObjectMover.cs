using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperObjectMover(
    IParser parser)
    : IObjectMover
{
    public void ExecuteDirection(ref OopContext context, Vector vector)
    {
        var count = parser.ReadNumber(context.Index, ref context.Actor.Instruction);
        if (count < 0)
            count = 1;

        if (context.Command == 0x3F) // ?
            count = -count;

        context.Actor.P2 = unchecked((byte)count);
        context.Actor.Vector = vector;
        context.Repeat = false;
    }
        
}