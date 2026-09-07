using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IObjectMover
{
    /// <remarks>
    /// RoZ: OopExecute (move/try shorthand commands)
    /// </remarks>
    void ExecuteDirection(ref OopContext context, Vector vector);
}