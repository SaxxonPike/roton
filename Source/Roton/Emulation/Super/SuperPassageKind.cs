using Roton.Emulation.Data;
using Roton.Emulation.Kinds;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super, 0x0B)]
public class SuperPassageKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xF0;
        element.Color = 0xFE;
        element.Cycle = 0;
        element.IsAlwaysVisible = true;
        element.MenuIndex = 1;
        element.MenuKey = 'P';
        element.Name = "Passage";
        element.BoardEditText = "Room thru passage?";
    }
}