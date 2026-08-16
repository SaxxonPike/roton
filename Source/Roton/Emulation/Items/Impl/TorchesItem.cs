using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "TORCHES")]
public sealed class TorchesItem(IEngineAccessor engine) : IItem
{
    private IEngine Engine => engine.Instance;

    public int Value
    {
        get => Engine.World.Torches;
        set => Engine.World.Torches = value;
    }
}