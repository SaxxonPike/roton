using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x1B)]
[Context(Context.Super, 0x1B)]
internal sealed class FakeWallKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xB2;
        element.MenuIndex = 3;
        element.IsEditorFloor = true;
        element.IsFloor = true;
        element.MenuKey = 'A';
        element.Name = "Fake";
    }
}