using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalColorMatcher : IColorMatcher
{
    public int GetColorMatchValue(int color) =>
        color;
}