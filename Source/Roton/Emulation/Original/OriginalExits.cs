using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalExits(IMemory memory) : IExits
{
    public ref HWord this[int index] =>
        ref memory.GetRef<HWord>(0x4569 + index);

    public ref HWord East =>
        ref memory.GetRef<HWord>(0x456C);

    public ref HWord North =>
        ref memory.GetRef<HWord>(0x4569);

    public ref HWord South =>
        ref memory.GetRef<HWord>(0x456A);

    public ref HWord West =>
        ref memory.GetRef<HWord>(0x456B);
}