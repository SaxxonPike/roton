using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x31)]
[Context(Context.Super, id: 0x4B)]
internal sealed class CyanTextKind : IKind
{
    public string FriendlyName => "Text (Cyan)";

    public void Initialize(IElement element)
    {
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}