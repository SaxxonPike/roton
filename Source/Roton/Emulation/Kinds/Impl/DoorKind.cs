using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x09)]
[Context(Context.Super, 0x09)]
public class DoorKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x0A;
        element.MenuIndex = 1;
        element.MenuKey = 'D';
        element.Name = "Door";
    }
}