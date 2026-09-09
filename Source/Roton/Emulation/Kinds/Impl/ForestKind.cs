using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x14)]
[Context(Context.Super, id: 0x14)]
internal sealed class ForestKind : IKind
{
    public string FriendlyName => "Forest";

    public void Initialize(IElement element)
    {
        element.Character = 0xB0;
        element.Color = 0x20;
        element.IsFloor = false;
        element.MenuIndex = 3;
        element.MenuKey = 'F';
        element.Name = "Forest";
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}