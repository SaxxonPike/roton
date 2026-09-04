using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x2D)]
[Context(Context.Super, 0x2D)]
internal sealed class CentipedeSegmentKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x4F;
        element.IsDestructible = true;
        element.Cycle = 2;
        element.MenuIndex = 2;
        element.MenuKey = 'S';
        element.Name = "Segment";
        element.Points = 3;
    }
}