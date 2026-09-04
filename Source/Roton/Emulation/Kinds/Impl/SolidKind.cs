using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x15)]
[Context(Context.Super, 0x15)]
public class SolidKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xDB;
        element.MenuIndex = 3;
        element.EditorCategory = "Walls:";
        element.MenuKey = 'S';
        element.Name = "Solid";
    }
}