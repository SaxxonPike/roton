using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x06)]
public class TorchKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x9D;
        element.Color = 0x06;
        element.IsAlwaysVisible = true;
        element.MenuIndex = 1;
        element.MenuKey = 'T';
        element.Name = "Torch";
    }
}