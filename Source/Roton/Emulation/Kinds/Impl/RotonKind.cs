using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Super, 0x3B)]
internal sealed class RotonKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x94;
        element.Color = 0x0D;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.Cycle = 1;
        element.MenuIndex = 4;
        element.MenuKey = 'R';
        element.Name = "Roton";
        element.EditorCategory = "Uglies:";
        element.P1EditText = "Intelligence?";
        element.P2EditText = "Switch Rate?";
        element.Points = 2;
    }
}