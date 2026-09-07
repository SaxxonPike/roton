using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Super, 0x2F)]
internal sealed class FloorKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xB0;
        element.MenuIndex = 5;
        element.IsEditorFloor = true;
        element.IsFloor = true;
        element.MenuKey = 'F';
        element.Name = "Floor";
        element.EditorCategory = "Terrains:";
    }
}