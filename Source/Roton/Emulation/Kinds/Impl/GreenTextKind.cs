using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x30)]
[Context(Context.Super, id: 0x4A)]
internal sealed class GreenTextKind : IKind
{
    public string FriendlyName => "Text (Green)";

    public void Initialize(IElement element)
    {
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}