using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Super, 0x40)]
public class StoneKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 'Z';
        element.Color = 0x0F;
        element.Cycle = 1;
        element.HasDrawCode = true;
        element.MenuIndex = 5;
        element.MenuKey = 'Z';
        element.Name = "Stone";
    }
}