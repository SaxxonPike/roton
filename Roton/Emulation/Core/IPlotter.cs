using Roton.Emulation.Data;

namespace Roton.Emulation.Core
{
    public interface IPlotter
    {
        void Put(IXyPair location, IXyPair vector, ITile kind);
        void PlotTile(IXyPair location, ITile tile);
        void ForcePlayerColor(int index);
    }
}