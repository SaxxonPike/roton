using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x11)]
[Context(Context.Super, id: 0x11)]
internal sealed class CounterClockwiseConveyorKind : IKind
{
    public string FriendlyName => "Conveyor (Counter-Clockwise)";

    public void Initialize(IElement element)
    {
        element.Character = '\\';
        element.Cycle = 2;
        element.HasDrawCode = true;
        element.MenuIndex = 1;
        element.MenuKey = '2';
        element.Name = "Counter";
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}