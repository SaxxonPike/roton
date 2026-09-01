using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperFeatures(
    IEngineAccessor engine,
    IState state)
    : IFeatures
{
    private IEngine Engine => engine.Instance;

    public void CleanUpOop(ref OopContext context)
    {
        var location = context.Actor.Location;
        Engine.PlotTile(location, context.DeathTile);
    }

    public string[] GetMessageLines()
    {
        return string.IsNullOrEmpty(state.Message2)
            ? [string.Empty, state.Message]
            : [state.Message, state.Message2];
    }

    public int BaseMemoryUsage => 203044;
}