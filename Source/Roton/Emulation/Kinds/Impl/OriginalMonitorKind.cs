using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x03)]
public class OriginalMonitorKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x20;
        element.Color = 0x07;
        element.Cycle = 1;
        element.Name = "Monitor";
    }
}