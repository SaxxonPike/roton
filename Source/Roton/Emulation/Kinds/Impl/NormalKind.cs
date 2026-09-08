using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x16)]
[Context(Context.Super, 0x16)]
internal sealed class NormalKind : IKind
{
    public string FriendlyName => "Normal Wall";

    public void Initialize(IElement element)
    {
        element.Character = 0xB2;
        element.MenuIndex = 3;
        element.MenuKey = 'N';
        element.Name = "Normal";
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}