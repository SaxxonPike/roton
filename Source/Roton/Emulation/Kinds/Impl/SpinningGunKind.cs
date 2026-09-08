using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x27)]
[Context(Context.Super, 0x27)]
internal sealed class SpinningGunKind : IKind
{
    public string FriendlyName => "Spinning Gun";

    public void Initialize(IElement element)
    {
        element.Character = 0x18;
        element.Cycle = 2;
        element.HasDrawCode = true;
        element.MenuIndex = 2;
        element.MenuKey = 'G';
        element.Name = "Spinning gun";
        element.P1EditText = "Intelligence?";
        element.P2EditText = "Firing rate?";
        element.P3EditText = "Firing type?";
    }

    public IEnumerable<EditorParam> GetEditorParams() =>
    [
        new("Intelligence", nameof(IActor.P1)),
        new("Firing rate", nameof(IActor.P2), EditorParamType.Bits06),
        new("Firing type", nameof(IActor.P2), EditorParamType.Bit7)
    ];
}