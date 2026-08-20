using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Super, "Z")]
public sealed class ZItem(IEngineAccessor engine) : IItem
{
    public ref Word Value => ref engine.Instance.World.Stones;
}