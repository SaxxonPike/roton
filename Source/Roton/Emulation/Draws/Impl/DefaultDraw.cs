using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class DefaultDraw : IDraw
{
    /// <remarks>
    /// RoZ: ElementDefaultDraw
    /// </remarks>
    public AnsiChar Draw(Location location) => 
        new(0x3F, 0x40);
}