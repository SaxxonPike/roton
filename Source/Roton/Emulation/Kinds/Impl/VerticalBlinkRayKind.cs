using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x2B)]
[Context(Context.Super, 0x47)]
internal sealed class VerticalBlinkRayKind : IKind
{
    public string FriendlyName => "Blink Ray (Vertical)";

    public void Initialize(IElement element)
    {
        element.Character = 0xBA;
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}