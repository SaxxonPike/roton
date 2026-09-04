using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Super, 0x22)]
public class SuperBearKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xEB;
        element.Color = 0x02;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.Cycle = 3;
        element.MenuIndex = 2;
        element.MenuKey = 'B';
        element.Name = "Bear";
        element.EditorCategory = "Creatures:";
        element.P1EditText = "Sensitivity?";
        element.Points = 1;
    }
}