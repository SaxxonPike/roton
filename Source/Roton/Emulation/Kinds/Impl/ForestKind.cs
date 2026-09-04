using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x14)]
[Context(Context.Super, 0x14)]
internal sealed class ForestKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xB0;
        element.Color = 0x20;
        element.IsFloor = false;
        element.MenuIndex = 3;
        element.MenuKey = 'F';
        element.Name = "Forest";
    }
}