using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Super, "Z")]
public sealed class ZItem(Lazy<IEngine> engine) : IItem
{
    private IEngine Engine => engine.Value;

    public int Value
    {
        get => Engine.World.Stones;
        set => Engine.World.Stones = value;
    }
}