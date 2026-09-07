using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x08)]
[Context(Context.Super, 0x08)]
internal sealed class KeyKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x0C;
        element.IsPushable = true;
        element.MenuIndex = 1;
        element.MenuKey = 'K';
        element.Name = "Key";
    }
}