using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "AMMO")]
[Context(Context.Super, "AMMO")]
public sealed class AmmoItem(IEngineAccessor engine) : IItem
{
    private IEngine Engine => engine.Instance;

    public int Value
    {
        get => Engine.World.Ammo;
        set => Engine.World.Ammo = value;
    }
}