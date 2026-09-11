using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Emulation.Data.Impl;

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