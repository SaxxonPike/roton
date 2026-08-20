using Roton.Emulation.Data;

namespace Roton.Emulation.Directions;

public interface IDirection
{
    Vector Execute(ref OopContext context, ref Word instruction);
}