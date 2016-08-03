namespace Roton.Core
{
    internal interface IRandomizer
    {
        int GetNext(int exclusiveUpperBound);
    }
}