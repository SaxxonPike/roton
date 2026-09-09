using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x07)]
[Context(Context.Super, id: 0x07)]
internal sealed class GemKind : IKind
{
    public string FriendlyName => "Gem";

    public void Initialize(IElement element)
    {
        element.Character = 0x04;
        element.IsPushable = true;
        element.IsDestructible = true;
        element.MenuIndex = 1;
        element.MenuKey = 'G';
        element.Name = "Gem";
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}