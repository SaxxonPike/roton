using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Super, id: 0x3D)]
internal sealed class PairerKind : IKind
{
    public string FriendlyName => "Pairer";

    public void Initialize(IElement element)
    {
        element.Character = 0xE5;
        element.Color = 0x01;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.Cycle = 2;
        element.MenuIndex = 4;
        element.MenuKey = 'P';
        element.Name = "Pairer";
        element.P1EditText = "Intelligence?";
        element.Points = 2;
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}