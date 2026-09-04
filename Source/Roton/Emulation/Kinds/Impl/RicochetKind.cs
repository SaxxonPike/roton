using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x20)]
[Context(Context.Super, 0x20)]
public class RicochetKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = '*';
        element.Color = 0x0A;
        element.MenuIndex = 3;
        element.MenuKey = 'R';
        element.Name = "Ricochet";
    }
}