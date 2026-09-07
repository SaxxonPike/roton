using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x18)]
[Context(Context.Super, 0x18)]
internal sealed class BoulderKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xFE;
        element.IsPushable = true;
        element.MenuIndex = 3;
        element.MenuKey = 'O';
        element.Name = "Boulder";
    }
}