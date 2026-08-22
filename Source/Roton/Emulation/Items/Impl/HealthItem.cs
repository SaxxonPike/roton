using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "HEALTH")]
[Context(Context.Super, "HEALTH")]
public sealed class HealthItem(
    IWorld world)
    : IItem
{
    public ref Word Value => ref world.Health;
}