namespace Roton.Core
{
    public interface IRandomizer
    {
        int GetNext(int exclusiveUpperBound);
    }
}