using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "AMMO")]
[Context(Context.Super, "AMMO")]
public sealed class AmmoItem(Lazy<IEngine> engine) : IItem
{
    private IEngine Engine => engine.Value;

    public int Value
    {
        get => Engine.World.Ammo;
        set => Engine.World.Ammo = value;
    }
}