using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "ZAP")]
[Context(Context.Super, "ZAP")]
internal sealed class ZapCheat(
    IEngineAccessor engine,
    IActorList actorList) : ICheat
{
    private IEngine Engine => engine.Instance;

    public void Execute(bool clear)
    {
        for (var i = 0; i < 4; i++)
        {
            Engine.Destroy(actorList.Player.Location + Engine.GetCardinalVector(i));
        }
    }
}