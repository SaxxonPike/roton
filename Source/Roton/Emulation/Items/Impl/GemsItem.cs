using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "GEMS")]
[Context(Context.Super, "GEMS")]
public sealed class GemsItem(IEngineAccessor engine) : IItem
{
    public ref Word Value => ref engine.Instance.World.Gems;
}