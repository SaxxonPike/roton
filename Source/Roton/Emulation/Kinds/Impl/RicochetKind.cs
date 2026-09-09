using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x20)]
[Context(Context.Super, id: 0x20)]
internal sealed class RicochetKind : IKind
{
    public string FriendlyName => "Ricochet";

    public void Initialize(IElement element)
    {
        element.Character = '*';
        element.Color = 0x0A;
        element.MenuIndex = 3;
        element.MenuKey = 'R';
        element.Name = "Ricochet";
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}