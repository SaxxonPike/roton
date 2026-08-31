using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IMover
{
    void MoveActor(int index, Location location);
    void MoveActorOnRiver(int index);
}