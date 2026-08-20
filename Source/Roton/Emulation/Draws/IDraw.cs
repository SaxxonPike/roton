using Roton.Emulation.Data;

namespace Roton.Emulation.Draws;

public interface IDraw
{
    AnsiChar Draw(Location location);
}