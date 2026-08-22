using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Super, "Z")]
public sealed class ZItem(
    IWorld world)
    : IItem
{
    public ref Word Value => ref world.Stones;
}