using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x00)]
[Context(Context.Super, 0x00)]
internal sealed class EmptyKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = ' ';
        element.Color = 0x70;
        element.IsPushable = true;
        element.IsFloor = true;
        element.Name = "Empty";
    }
}