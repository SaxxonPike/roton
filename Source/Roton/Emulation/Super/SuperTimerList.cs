using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperTimerList(IMemory memory) : ITimerList
{
    public ITimer Player { get; } = new Timer(memory, 0xB95E);
    public ITimer TimeLimit { get; } = new Timer(memory, 0x79CA);
}