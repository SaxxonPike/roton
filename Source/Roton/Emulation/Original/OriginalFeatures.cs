using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalFeatures(
    IEngineAccessor engine)
    : IFeatures
{
    private IEngine Engine => engine.Instance;

    public void CleanUpOop(ref OopContext context)
    {
        var location = context.Actor.Location;
        Engine.Harm(context.Index);
        Engine.PlotTile(location, context.DeathTile);
    }
}