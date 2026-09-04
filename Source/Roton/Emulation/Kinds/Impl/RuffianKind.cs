using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x23)]
[Context(Context.Super, 0x23)]
public class RuffianKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x05;
        element.Color = 0x0D;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.Cycle = 1;
        element.MenuIndex = 2;
        element.MenuKey = 'R';
        element.Name = "Ruffian";
        element.P1EditText = "Intelligence?";
        element.P2EditText = "Resting time?";
        element.Points = 2;
    }
}