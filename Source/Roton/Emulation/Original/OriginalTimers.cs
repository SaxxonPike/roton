using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalTimers(IMemory memory) : ITimers
{
    public ITimer Player { get; } = new MemoryTimer(memory, 0x740A);
    public ITimer TimeLimit { get; } = new MemoryTimer(memory, 0x4920);
}