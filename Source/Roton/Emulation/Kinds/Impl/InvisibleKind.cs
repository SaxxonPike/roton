using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x1C)]
[Context(Context.Super, id: 0x1C)]
internal sealed class InvisibleKind(IState state) : IKind
{
    public string FriendlyName => "Invisible Wall";

    public void Initialize(IElement element)
    {
        element.Character = state.EditorMode ? 0xB0 : 0x20;
        element.MenuIndex = 3;
        element.MenuKey = 'I';
        element.Name = "Invisible";
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}