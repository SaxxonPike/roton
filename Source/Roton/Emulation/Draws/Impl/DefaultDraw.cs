using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class DefaultDraw : IDraw
{
    public AnsiChar Draw(Location location)
    {
        return new AnsiChar(0x3F, 0x40);
    }
}