using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x0F)]
[Context(Context.Super, id: 0x48)]
internal sealed class StarKind : IKind
{
    public string FriendlyName => "Star";

    public void Initialize(IElement element)
    {
        element.Character = 0x53;
        element.Color = 0x0F;
        element.Cycle = 1;
        element.IsDestructible = false;
        element.HasDrawCode = true;
        element.Name = "Star";
    }

    public IEnumerable<EditorParam> GetEditorParams() =>
    [
        new("Action Cycle", nameof(IActor.P2), EditorParamType.YesNo)
    ];
}