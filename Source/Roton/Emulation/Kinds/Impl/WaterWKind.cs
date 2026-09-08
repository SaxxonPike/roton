using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Super, 0x32)]
internal sealed class WaterWKind : IKind
{
    public string FriendlyName => "Water (West)";

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

    public IEnumerable<EditorParam> GetEditorParams() => [];
}