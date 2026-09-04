using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x10)]
[Context(Context.Super, 0x10)]
public class ClockwiseConveyorKind : IKind
{
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
}