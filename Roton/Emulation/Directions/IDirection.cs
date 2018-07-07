using Roton.Core;

namespace Roton.Emulation.Directions
{
    public interface IDirection
    {
        string Name { get; }
        IXyPair Execute(IOopContext oopContext);
    }
}