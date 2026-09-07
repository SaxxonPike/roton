using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x29)]
[Context(Context.Super, 0x29)]
internal sealed class LionKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xEA;
        element.Color = 0x0C;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.Cycle = 2;
        element.MenuIndex = 2;
        element.MenuKey = 'L';
        element.Name = "Lion";
        element.EditorCategory = "Beasts:";
        element.P1EditText = "Intelligence?";
        element.Points = 1;
    }
}