using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "ZAP")]
[Context(Context.Super, "ZAP")]
public sealed class ZapCheat(IEngineAccessor engine) : ICheat
{
    private IEngine Engine => engine.Instance;

    public void Execute(ReadOnlySpan<char> name, bool clear)
    {
        for (var i = 0; i < 4; i++)
        {
            Engine.Destroy(Engine.Player.Location + Engine.GetCardinalVector(i));
        }
    }
}