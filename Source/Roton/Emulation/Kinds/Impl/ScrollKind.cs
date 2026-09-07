using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x0A)]
[Context(Context.Super, 0x0A)]
internal sealed class ScrollKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xE8;
        element.Color = 0x0F;
        element.IsPushable = true;
        element.Cycle = 1;
        element.MenuIndex = 1;
        element.MenuKey = 'S';
        element.Name = "Scroll";
        element.CodeEditText = "Edit text of scroll";
    }
}