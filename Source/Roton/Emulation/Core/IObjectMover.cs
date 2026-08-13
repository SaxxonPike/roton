using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IObjectMover
{
    void ExecuteDirection(IOopContext context, IXyPair vector);
}