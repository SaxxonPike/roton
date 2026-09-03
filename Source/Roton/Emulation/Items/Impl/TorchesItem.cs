using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "TORCHES")]
internal sealed class TorchesItem(
    IWorld world)
    : IItem
{
    public ref Word Value => ref world.Torches;
}