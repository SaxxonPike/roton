using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x25)]
[Context(Context.Super, id: 0x25)]
internal sealed class SlimeKind : IKind
{
    public string FriendlyName => "Slime";

    public void Initialize(IElement element)
    {
        element.Character = '*';
        element.Cycle = 3;
        element.IsDestructible = false;
        element.MenuIndex = 2;
        element.MenuKey = 'V';
        element.Name = "Slime";
        element.P2EditText = "Movement speed?;FS";
    }

    public IEnumerable<EditorParam> GetEditorParams() =>
    [
        new("Expansion Counter", nameof(IActor.P1)),
        new("Movement Speed", nameof(IActor.P2))
    ];
}