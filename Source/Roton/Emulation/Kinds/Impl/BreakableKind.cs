using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x17)]
[Context(Context.Super, id: 0x17)]
internal sealed class BreakableKind : IKind
{
    public string FriendlyName => "Breakable Wall";

    public void Initialize(IElement element)
    {
        element.Character = 0xB1;
        element.IsDestructible = false;
        element.MenuIndex = 3;
        element.MenuKey = 'B';
        element.Name = "Breakable";
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}