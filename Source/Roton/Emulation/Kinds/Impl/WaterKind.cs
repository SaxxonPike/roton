using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x13)]
public class WaterKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xB0;
        element.Color = 0xF9;
        element.IsEditorFloor = true;
        element.MenuIndex = 3;
        element.MenuKey = 'W';
        element.Name = "Water";
        element.EditorCategory = "Terrains:";
    }
}