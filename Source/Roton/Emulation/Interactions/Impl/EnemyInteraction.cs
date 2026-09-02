using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x0F)]
[Context(Context.Original, 0x12)]
[Context(Context.Original, 0x22)]
[Context(Context.Original, 0x23)]
[Context(Context.Original, 0x29)]
[Context(Context.Original, 0x2A)]
[Context(Context.Original, 0x2C)]
[Context(Context.Original, 0x2D)]
[Context(Context.Super, 0x22)]
[Context(Context.Super, 0x23)]
[Context(Context.Super, 0x29)]
[Context(Context.Super, 0x2A)]
[Context(Context.Super, 0x2C)]
[Context(Context.Super, 0x2D)]
[Context(Context.Super, 0x3B)]
[Context(Context.Super, 0x3C)]
[Context(Context.Super, 0x3D)]
[Context(Context.Super, 0x3E)]
[Context(Context.Super, 0x45)]
[Context(Context.Super, 0x48)]
internal sealed class EnemyInteraction(
    IAttacker attacker) 
    : IInteraction
{
    public void Interact(Location location, int index, ref Vector vector) => 
        attacker.Attack(index, location);
}