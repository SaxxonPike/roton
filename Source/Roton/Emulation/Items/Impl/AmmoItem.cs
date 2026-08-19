using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "AMMO")]
[Context(Context.Super, "AMMO")]
public sealed class AmmoItem(IEngineAccessor engine) : IItem
{
    public ref Word Value => ref engine.Instance.World.Ammo;
}