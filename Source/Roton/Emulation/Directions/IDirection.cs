using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Directions;

public interface IDirection
{
    Vector Execute(IOopContext context);
}