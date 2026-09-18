using System.Collections.Generic;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x2B)]
[Context(Context.Super, id: 0x47)]
internal sealed class VerticalBlinkRayKind : IKind
{
    public string FriendlyName => "Blink Ray (Vertical)";

    public void Initialize(IElement element)
    {
        element.Character = 0xBA;
    }
}