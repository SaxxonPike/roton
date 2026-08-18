using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Interactions;

public interface IInteraction
{
    void Interact(Location location, int index, ref Vector vector);
}