namespace Roton.Emulation.Core;

public interface IRandomizer
{
    int GetNext(int exclusiveUpperBound);
}