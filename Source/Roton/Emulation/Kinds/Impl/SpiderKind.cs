using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Super, 0x3E)]
internal sealed class SpiderKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x0F;
        element.Color = 0xFF;
        element.IsDestructible = true;
        element.Cycle = 1;
        element.MenuIndex = 4;
        element.MenuKey = 'S';
        element.Name = "Spider";
        element.P1EditText = "Intelligence?";
        element.Points = 3;
    }
}