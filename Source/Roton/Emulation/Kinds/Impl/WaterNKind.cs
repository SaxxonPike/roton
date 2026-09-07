using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Super, 0x30)]
internal sealed class WaterNKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x1E;
        element.Color = 0x19;
        element.MenuIndex = 5;
        element.IsEditorFloor = true;
        element.IsFloor = true;
        element.MenuKey = '8';
        element.Name = "Water N";
    }
}