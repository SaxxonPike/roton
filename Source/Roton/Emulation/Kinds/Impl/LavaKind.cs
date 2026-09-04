using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Super, 0x13)]
public class LavaKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x6F;
        element.Color = 0x4E;
        element.IsEditorFloor = true;
        element.MenuIndex = 3;
        element.MenuKey = 'L';
        element.Name = "Lava";
        element.EditorCategory = "Terrains:";
    }
}