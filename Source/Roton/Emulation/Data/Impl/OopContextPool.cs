using Roton.Emulation.Core;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Data.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class OopContextPool(IEngine engine)
    : ObjectPool<IOopContext>(() => new OopContext(engine), Reset), IOopContextPool
{
    private static void Reset(IOopContext obj)
    {
        obj.InstructionSource = null;
        obj.CommandsExecuted = 0;
        obj.Moved = false;
        obj.Repeat = false;
        obj.Died = false;
        obj.Finished = false;
        obj.Executed = false;
        obj.NextLine = false;
        obj.PreviousInstruction = 0;
        obj.Resume = false;
        obj.SearchIndex = 0;
        obj.SearchOffset = 0;
        obj.Command = 0;
        obj.Message.Clear();
        obj.DeathTile?.SetTo(0, 0);
    }
}