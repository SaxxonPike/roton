using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IMover
{
    void Move(int index, Location location);
    void Float(int index);
}