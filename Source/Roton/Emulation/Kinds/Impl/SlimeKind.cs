using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x25)]
[Context(Context.Super, 0x25)]
internal sealed class SlimeKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = '*';
        element.Cycle = 3;
        element.MenuIndex = 2;
        element.MenuKey = 'V';
        element.Name = "Slime";
        element.P2EditText = "Movement speed?;FS";
    }
}