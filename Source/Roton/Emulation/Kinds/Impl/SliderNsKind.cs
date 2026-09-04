using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x19)]
[Context(Context.Super, 0x19)]
public class SliderNsKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x12;
        element.MenuIndex = 3;
        element.MenuKey = '1';
        element.Name = "Slider (NS)";
    }
}