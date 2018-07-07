namespace Roton.Core
{
    public interface IXyPair
    {
        int X { get; set; }
        int Y { get; set; }
        IXyPair Clone();
    }
}