using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x1A)]
[Context(Context.Super, id: 0x1A)]
internal sealed class SliderEwKind : IKind
{
    public string FriendlyName => "Slider (East-West)";

    public void Initialize(IElement element)
    {
        element.Character = 0x1D;
        element.MenuIndex = 3;
        element.MenuKey = '2';
        element.Name = "Slider (EW)";
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}