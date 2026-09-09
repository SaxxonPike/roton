using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x2C)]
[Context(Context.Super, id: 0x2C)]
internal sealed class CentipedeHeadKind : IKind
{
    public string FriendlyName => "Centipede (Head)";

    public void Initialize(IElement element)
    {
        element.Character = 0xE9;
        element.IsDestructible = true;
        element.Cycle = 2;
        element.MenuIndex = 2;
        element.MenuKey = 'H';
        element.Name = "Head";
        element.EditorCategory = "Centipedes";
        element.P1EditText = "Intelligence?";
        element.P2EditText = "Deviance?";
        element.Points = 1;
    }

    public IEnumerable<EditorParam> GetEditorParams() => [
        new("Intelligence", nameof(IActor.P1)),
        new("Deviance", nameof(IActor.P2)),
        new("Follower", nameof(IActor.Follower), EditorParamType.Actor)
    ];
}