using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x33)]
[Context(Context.Super, id: 0x4D)]
internal sealed class PurpleTextKind : IKind
{
    public string FriendlyName => "Text (Purple)";

    public void Initialize(IElement element)
    {
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}