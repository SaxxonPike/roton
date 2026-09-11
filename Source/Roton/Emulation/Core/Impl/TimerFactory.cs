using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class TimerFactory(
    IMemory memory,
    IConfig config)
    : ITimerFactory
{
    public ITimer Create(int offset) =>
        new Timer(memory, offset, config);
}