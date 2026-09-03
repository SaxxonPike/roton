using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperColorMatcher : IColorMatcher
{
    public int GetColorMatchValue(int color) => 
        color & 0x07;
}