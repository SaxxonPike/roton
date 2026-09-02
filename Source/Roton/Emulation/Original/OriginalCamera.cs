using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalCamera : ICamera
{
    public bool UpdateCamera()
    {
        // Original engine does not have a camera.
        return false;
    }
}