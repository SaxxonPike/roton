using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "TORCHES")]
public sealed class TorchesItem(IEngineAccessor engine) : IItem
{
    public ref Word Value => ref engine.Instance.World.Torches;
}