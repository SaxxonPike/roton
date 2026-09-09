using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x0C)]
[Context(Context.Super, id: 0x0C)]
internal sealed class DuplicatorKind : IKind
{
    public string FriendlyName => "Duplicator";

    public void Initialize(IElement element)
    {
        element.Character = 0xFA;
        element.Color = 0x0F;
        element.Cycle = 2;
        element.HasDrawCode = true;
        element.MenuIndex = 1;
        element.MenuKey = 'U';
        element.Name = "Duplicator";
        element.StepEditText = "Source direction?";
        element.P2EditText = "Duplication rate?;SF";
    }

    public IEnumerable<EditorParam> GetEditorParams() => [
        new("Source Direction", nameof(IActor.Vector)),
        new("Duplication Rate", nameof(IActor.P2))
    ];
}