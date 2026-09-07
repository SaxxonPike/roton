using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x1F)]
[Context(Context.Super, 0x1F)]
internal sealed class LineKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 206;
        element.HasDrawCode = true;
        element.Name = "Line";
    }
}