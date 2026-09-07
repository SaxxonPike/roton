using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x1A)]
[Context(Context.Super, 0x1A)]
internal sealed class SliderEwKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x1D;
        element.MenuIndex = 3;
        element.MenuKey = '2';
        element.Name = "Slider (EW)";
    }
}