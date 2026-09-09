using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x21)]
[Context(Context.Super, id: 0x46)]
internal sealed class HorizontalBlinkRayKind : IKind
{
    public string FriendlyName => "Blink Ray (Horizontal)";

    public void Initialize(IElement element)
    {
        element.Character = 0xCD;
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}