using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x10)]
[Context(Context.Super, id: 0x10)]
internal sealed class ClockwiseConveyorKind : IKind
{
    public string FriendlyName => "Conveyor (Clockwise)";

    public void Initialize(IElement element)
    {
        element.Character = '/';
        element.Cycle = 3;
        element.HasDrawCode = true;
        element.MenuIndex = 1;
        element.MenuKey = '1';
        element.Name = "Clockwise";
        element.EditorCategory = "Conveyors:";
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}