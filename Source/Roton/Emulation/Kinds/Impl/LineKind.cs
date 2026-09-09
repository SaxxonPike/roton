using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x1F)]
[Context(Context.Super, id: 0x1F)]
internal sealed class LineKind : IKind
{
    public string FriendlyName => "Line Wall";

    public void Initialize(IElement element)
    {
        element.Character = 206;
        element.HasDrawCode = true;
        element.Name = "Line";
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}