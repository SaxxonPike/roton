using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "ZAP")]
[Context(Context.Super, "ZAP")]
public sealed class ZapCheat : ICheat
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public ZapCheat(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public void Execute(string name, bool clear)
    {
        for (var i = 0; i < 4; i++)
        {
            Engine.Destroy(Engine.Player.Location.Sum(Engine.GetCardinalVector(i)));
        }
    }
}