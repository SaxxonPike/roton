using Roton.Emulation.Data;

namespace Roton.Emulation.Directions
{
    public interface IDirection
    {
        string Name { get; }
        IXyPair Execute(IOopContext context);
    }
}