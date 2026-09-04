using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Super, 0x3C)]
internal sealed class DragonPupKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xED;
        element.Color = 0x04;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.Cycle = 2;
        element.HasDrawCode = true;
        element.MenuIndex = 4;
        element.MenuKey = 'D';
        element.Name = "Dragon Pup";
        element.P1EditText = "Intelligence?";
        element.P2EditText = "Switch Rate?";
        element.Points = 1;
    }
}