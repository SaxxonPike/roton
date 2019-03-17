using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Draws
{
    public interface IDraw
    {
        AnsiChar Draw(IXyPair location);
    }
}