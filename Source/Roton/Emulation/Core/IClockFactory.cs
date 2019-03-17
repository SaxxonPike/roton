namespace Roton.Emulation.Core
{
    public interface IClockFactory
    {
        IClock Create(long numerator, long denominator);
    }
}