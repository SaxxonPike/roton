using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Emulation.Kinds;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super, 0x24)]
internal sealed class SuperObjectKind : IKind
{
    public string FriendlyName => "Object";

    public void Initialize(IElement element)
    {
        element.Character = 0x02;
        element.MenuIndex = 2;
        element.Cycle = 3;
        element.HasDrawCode = true;
        element.MenuKey = 'O';
        element.Name = "Object";
        element.P1EditText = "Character?";
        element.CodeEditText = "Edit Program";
    }

    public IEnumerable<EditorParam> GetEditorParams() =>
    [
        new("Character", nameof(IActor.P1), EditorParamType.Character),
        new("Steps", nameof(IActor.P2)),
        new("Locked", nameof(IActor.P3), EditorParamType.YesNo)
    ];
}