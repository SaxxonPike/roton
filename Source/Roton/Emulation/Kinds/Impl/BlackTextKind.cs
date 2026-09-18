using System.Collections.Generic;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x35)]
[Context(Context.Super, id: 0x4F)]
internal sealed class BlackTextKind : IKind
{
    public string FriendlyName => "Text (Black)";

    public void Initialize(IElement element)
    {
    }
}