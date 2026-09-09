using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x2A)]
[Context(Context.Super, id: 0x2A)]
internal sealed class TigerKind : IKind
{
    public string FriendlyName => "Tiger";

    public void Initialize(IElement element)
    {
        element.Character = 0xE3;
        element.Color = 0x0B;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.Cycle = 2;
        element.MenuIndex = 2;
        element.MenuKey = 'T';
        element.Name = "Tiger";
        element.P1EditText = "Intelligence?";
        element.P2EditText = "Firing rate?";
        element.P3EditText = "Firing type?";
        element.Points = 2;
    }

    public IEnumerable<EditorParam> GetEditorParams() =>
    [
        new("Intelligence", nameof(IActor.P1)),
        new("Firing rate", nameof(IActor.P2), EditorParamType.Bits06),
        new("Firing type", nameof(IActor.P2), EditorParamType.Bit7)
    ];
}