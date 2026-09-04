using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Super, 0x33)]
public class WaterEKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x10;
        element.Color = 0x19;
        element.MenuIndex = 5;
        element.IsEditorFloor = true;
        element.IsFloor = true;
        element.MenuKey = '6';
        element.Name = "Water E";
    }
}