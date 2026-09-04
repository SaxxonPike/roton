using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x2B)]
[Context(Context.Super, 0x47)]
internal sealed class VerticalBlinkWallKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xBA;
    }
}