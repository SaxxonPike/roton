using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public class OriginalForestHandler(
    ITileRemover tileRemover)
    : IForestHandler
{
    public void ClearForest(Location location) =>
        tileRemover.RemoveItem(location);
}