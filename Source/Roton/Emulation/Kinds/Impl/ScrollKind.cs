using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x0A)]
[Context(Context.Super, id: 0x0A)]
internal sealed class ScrollKind : IKind
{
    public string FriendlyName => "Scroll";

    public void Initialize(IElement element)
    {
        element.Character = 0xE8;
        element.Color = 0x0F;
        element.IsPushable = true;
        element.Cycle = 1;
        element.MenuIndex = 1;
        element.MenuKey = 'S';
        element.Name = "Scroll";
        element.CodeEditText = "Edit text of scroll";
    }

    public IEnumerable<EditorParam> GetEditorParams() =>
    [
        new("Code", nameof(IActor.Code), EditorParamType.Code)
    ];
}