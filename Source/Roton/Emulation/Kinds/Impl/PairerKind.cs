using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Super, 0x3D)]
internal sealed class PairerKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xE5;
        element.Color = 0x01;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.Cycle = 2;
        element.MenuIndex = 4;
        element.MenuKey = 'P';
        element.Name = "Pairer";
        element.P1EditText = "Intelligence?";
        element.Points = 2;
    }
}