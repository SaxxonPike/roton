using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x26)]
internal sealed class SharkKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = '^';
        element.Color = 0x07;
        element.Cycle = 3;
        element.MenuIndex = 2;
        element.MenuKey = 'Y';
        element.Name = "Shark";
        element.P1EditText = "Intelligence?";
    }
}