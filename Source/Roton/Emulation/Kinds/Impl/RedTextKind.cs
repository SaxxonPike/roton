using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x32)]
[Context(Context.Super, id: 0x4C)]
internal sealed class RedTextKind : IKind
{
    public string FriendlyName => "Text (Red)";

    public void Initialize(IElement element)
    {
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}