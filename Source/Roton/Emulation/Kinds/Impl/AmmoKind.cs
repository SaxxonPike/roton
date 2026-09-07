using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x05)]
[Context(Context.Super, 0x05)]
internal sealed class AmmoKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x84;
        element.Color = 0x03;
        element.IsPushable = true;
        element.MenuIndex = 1;
        element.MenuKey = 'A';
        element.Name = "Ammo";
    }
}