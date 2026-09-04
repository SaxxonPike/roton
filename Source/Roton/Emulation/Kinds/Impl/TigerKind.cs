using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x2A)]
[Context(Context.Super, 0x2A)]
public class TigerKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xE3;
        element.Color = 0x0B;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.Cycle = 2;
        element.MenuIndex = 2;
        element.MenuKey = 'T';
        element.Name = "Tiger";
        element.P1EditText = "Intelligence?";
        element.P2EditText = "Firing rate?";
        element.P3EditText = "Firing type?";
        element.Points = 2;
    }
}