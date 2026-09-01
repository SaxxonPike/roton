using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperExits(IMemory memory) : IExits
{
    public ref HWord this[int index] => ref memory.GetRef<HWord>(0x7768 + index);

    public ref HWord East => ref memory.GetRef<HWord>(0x776B);

    public ref HWord North => ref memory.GetRef<HWord>(0x7768);

    public ref HWord South => ref memory.GetRef<HWord>(0x7769);

    public ref HWord West => ref memory.GetRef<HWord>(0x776A);
}