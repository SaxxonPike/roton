using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Super, 0x32)]
public class WaterWKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x11;
        element.Color = 0x19;
        element.MenuIndex = 5;
        element.IsEditorFloor = true;
        element.IsFloor = true;
        element.MenuKey = '4';
        element.Name = "Water W";
    }
}