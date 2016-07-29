namespace Roton.Emulation.Execution
{
    internal interface IRandomizer
    {
        int GetNext(int exclusiveUpperBound);
    }
}