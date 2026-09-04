using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x07)]
[Context(Context.Super, 0x07)]
public class GemKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x04;
        element.IsPushable = true;
        element.IsDestructible = true;
        element.MenuIndex = 1;
        element.MenuKey = 'G';
        element.Name = "Gem";
    }
}