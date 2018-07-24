using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class ClockFactory : IClockFactory
    {
        public IClock Create(long numerator, long denominator)
        {
            return new Clock(numerator, denominator);
        }
    }
}