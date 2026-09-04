using Roton.Emulation.Data;
using Roton.Emulation.Kinds;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super, 0x03)]
public class SuperMonitorKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x02;
        element.Color = 0x1F;
        element.Cycle = 1;
        element.IsPushable = true;
        element.Name = "Monitor";
    }
}