using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalTimers(IMemory memory) : ITimers
{
    public ITimer Player { get; } = new Timer(memory, 0x740A);
    public ITimer TimeLimit { get; } = new Timer(memory, 0x4920);
}