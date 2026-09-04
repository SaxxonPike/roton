using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x1C)]
[Context(Context.Super, 0x1C)]
internal sealed class InvisibleKind(IState state) : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = state.EditorMode ? 0xB0 : 0x20;
        element.MenuIndex = 3;
        element.MenuKey = 'I';
        element.Name = "Invisible";
    }
}