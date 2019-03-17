using Roton.Emulation.Data;

namespace Roton.Emulation.Interactions
{
    public interface IInteraction
    {
        void Interact(IXyPair location, int index, IXyPair vector);
    }
}