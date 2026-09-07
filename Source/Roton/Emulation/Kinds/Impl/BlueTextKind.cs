using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x2F)]
[Context(Context.Super, 0x49)]
internal sealed class BlueTextKind : IKind
{
    public void Initialize(IElement element)
    {
    }
}