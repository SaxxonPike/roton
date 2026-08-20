using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "TIME")]
[Context(Context.Super, "TIME")]
public sealed class TimeItem(IEngineAccessor engine) : IItem
{
    public ref Word Value => ref engine.Instance.World.TimePassed;
}