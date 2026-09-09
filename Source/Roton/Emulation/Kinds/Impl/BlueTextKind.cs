using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x2F)]
[Context(Context.Super, id: 0x49)]
internal sealed class BlueTextKind : IKind
{
    public string FriendlyName => "Text (Blue)";

    public void Initialize(IElement element)
    {
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}