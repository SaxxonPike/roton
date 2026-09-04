using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x0E)]
[Context(Context.Super, 0x0E)]
internal sealed class EnergizerKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x7F;
        element.Color = 0x05;
        element.MenuIndex = 1;
        element.MenuKey = 'E';
        element.Name = "Energizer";
    }
}