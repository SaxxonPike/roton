using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Draws;

public interface IDraw
{
    AnsiChar Draw(Location location);
}