using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x22)]
public class OriginalBearKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x99;
        element.Color = 0x06;
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