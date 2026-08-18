using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Core;

public interface IObjectMover
{
    void ExecuteDirection(ref OopContext context, Vector vector);
}