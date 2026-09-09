using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x34)]
[Context(Context.Super, id: 0x4E)]
internal sealed class BrownTextKind : IKind
{
    public string FriendlyName => "Text (Brown)";

    public void Initialize(IElement element)
    {
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}