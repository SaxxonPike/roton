using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "ZAP")]
[Context(Context.Super, "ZAP")]
public sealed class ZapCheat(Lazy<IEngine> engine) : ICheat
{
    private IEngine Engine => engine.Value;

    public void Execute(ReadOnlySpan<char> name, bool clear)
    {
        for (var i = 0; i < 4; i++)
        {
            Engine.Destroy(Engine.Player.Location.Sum(Engine.GetCardinalVector(i)));
        }
    }
}