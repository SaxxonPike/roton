using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Emulation.Kinds;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super, id: 0x22)]
internal sealed class SuperBearKind : IKind
{
    public string FriendlyName => "Bear";

    public void Initialize(IElement element)
    {
        element.Character = 0xEB;
        element.Color = 0x02;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.Cycle = 3;
        element.MenuIndex = 2;
        element.MenuKey = 'B';
        element.Name = "Bear";
        element.EditorCategory = "Creatures:";
        element.P1EditText = "Sensitivity?";
        element.Points = 1;
    }

    public IEnumerable<EditorParam> GetEditorParams() =>
    [
        new("Sensitivity", nameof(IActor.P1))
    ];
}