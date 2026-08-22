using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "AMMO")]
[Context(Context.Super, "AMMO")]
public sealed class AmmoItem(
    IWorld world)
    : IItem
{
    public ref Word Value => ref world.Ammo;
}