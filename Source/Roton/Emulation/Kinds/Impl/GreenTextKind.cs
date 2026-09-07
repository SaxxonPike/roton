using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x30)]
[Context(Context.Super, 0x4A)]
internal sealed class GreenTextKind : IKind
{
    public void Initialize(IElement element)
    {
    }
}