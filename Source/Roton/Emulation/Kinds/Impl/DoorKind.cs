using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x09)]
[Context(Context.Super, id: 0x09)]
internal sealed class DoorKind : IKind
{
    public string FriendlyName => "Door";

    public void Initialize(IElement element)
    {
        element.Character = 0x0A;
        element.Color = 0xFE;
        element.MenuIndex = 1;
        element.MenuKey = 'D';
        element.Name = "Door";
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}