using Roton.Emulation.Directions;

namespace Roton.Emulation.Commands
{
    public interface IDirections
    {
        IDirection Get(string name);
    }
}