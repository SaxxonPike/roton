using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class ClockFactory : IClockFactory
{
    public IClock Create(long numerator, long denominator)
    {
        return new Clock(numerator, denominator);
    }
}