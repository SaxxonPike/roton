using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Super, 0x31)]
internal sealed class WaterSKind : IKind
{
    public string FriendlyName => "Water (South)";

    public void Initialize(IElement element)
    {
        element.Character = 0x1F;
        element.Color = 0x19;
        element.MenuIndex = 5;
        element.IsEditorFloor = true;
        element.IsFloor = true;
        element.MenuKey = '2';
        element.Name = "Water S";
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}