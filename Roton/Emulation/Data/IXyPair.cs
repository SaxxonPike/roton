namespace Roton.Emulation.Data
{
    public interface IXyPair
    {
        int X { get; set; }
        int Y { get; set; }
        IXyPair Clone();
    }
}