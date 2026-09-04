using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x0F)]
[Context(Context.Super, 0x48)]
internal sealed class StarKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x53;
        element.Color = 0x0F;
        element.Cycle = 1;
        element.IsDestructible = false;
        element.HasDrawCode = true;
        element.Name = "Star";
    }
}