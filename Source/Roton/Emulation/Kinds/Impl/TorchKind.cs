using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x06)]
internal sealed class TorchKind : IKind
{
    public string FriendlyName => "Torch";

    public void Initialize(IElement element)
    {
        element.Character = 0x9D;
        element.Color = 0x06;
        element.IsAlwaysVisible = true;
        element.MenuIndex = 1;
        element.MenuKey = 'T';
        element.Name = "Torch";
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}