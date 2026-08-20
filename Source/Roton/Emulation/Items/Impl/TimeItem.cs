using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "TIME")]
[Context(Context.Super, "TIME")]
public sealed class TimeItem(IEngineAccessor engine) : IItem
{
    public ref Word Value => ref engine.Instance.World.TimePassed;
}