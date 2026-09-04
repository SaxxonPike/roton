using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x12)]
[Context(Context.Super, 0x45)]
public class BulletKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xF8;
        element.Color = 0x0F;
        element.IsDestructible = true;
        element.Cycle = 1;
        element.Name = "Bullet";
    }
}