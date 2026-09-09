using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x08)]
[Context(Context.Super, id: 0x08)]
internal sealed class KeyKind : IKind
{
    public string FriendlyName => "Key";

    public void Initialize(IElement element)
    {
        element.Character = 0x0C;
        element.IsPushable = true;
        element.MenuIndex = 1;
        element.MenuKey = 'K';
        element.Name = "Key";
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}