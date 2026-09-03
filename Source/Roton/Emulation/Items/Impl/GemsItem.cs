using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "GEMS")]
[Context(Context.Super, "GEMS")]
internal sealed class GemsItem(
    IWorld world)
    : IItem
{
    public ref Word Value =>
        ref world.Gems;
}