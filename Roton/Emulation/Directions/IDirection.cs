using Roton.Emulation.Data;

namespace Roton.Emulation.Directions
{
    public interface IDirection
    {
        IXyPair Execute(IOopContext context);
    }
}