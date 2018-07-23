namespace Roton.Emulation.Core.Impl
{
    public sealed class ClockFactory : IClockFactory
    {
        public IClock Create(long numerator, long denominator)
        {
            return new Clock(numerator, denominator);
        }
    }
}