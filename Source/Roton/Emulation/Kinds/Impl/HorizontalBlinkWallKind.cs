using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x21)]
[Context(Context.Super, 0x46)]
public class HorizontalBlinkWallKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xCD;
    }
}