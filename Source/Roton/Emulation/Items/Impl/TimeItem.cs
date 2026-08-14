using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "TIME")]
[Context(Context.Super, "TIME")]
public sealed class TimeItem(Lazy<IEngine> engine) : IItem
{
    private IEngine Engine => engine.Value;

    public int Value
    {
        get => Engine.World.TimePassed;
        set => Engine.World.TimePassed = value;
    }
}