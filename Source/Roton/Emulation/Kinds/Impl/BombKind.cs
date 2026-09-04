using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x0D)]
[Context(Context.Super, 0x0D)]
internal sealed class BombKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x0B;
        element.HasDrawCode = true;
        element.IsPushable = true;
        element.Cycle = 6;
        element.MenuIndex = 1;
        element.MenuKey = 'B';
        element.Name = "Bomb";
    }
}