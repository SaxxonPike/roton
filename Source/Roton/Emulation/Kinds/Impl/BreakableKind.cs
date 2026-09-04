using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x17)]
[Context(Context.Super, 0x17)]
public class BreakableKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xB1;
        element.MenuIndex = 3;
        element.MenuKey = 'B';
        element.Name = "Breakable";
    }
}