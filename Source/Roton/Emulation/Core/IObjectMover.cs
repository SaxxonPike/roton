using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Core;

public interface IObjectMover
{
    void ExecuteDirection(IOopContext context, Vector vector);
}