using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x04)]
public class OriginalPlayerKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x02;
        element.Color = 0x1F;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.IsAlwaysVisible = true;
        element.Cycle = 1;
        element.MenuIndex = 1;
        element.MenuKey = 'Z';
        element.Name = "Player";
        element.EditorCategory = "Items:";
    }
}