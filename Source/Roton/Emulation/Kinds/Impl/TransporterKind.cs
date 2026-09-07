using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x1E)]
[Context(Context.Super, 0x1E)]
internal sealed class TransporterKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xC5;
        element.HasDrawCode = true;
        element.Cycle = 2;
        element.MenuIndex = 3;
        element.MenuKey = 'T';
        element.Name = "Transporter";
        element.StepEditText = "Direction?";
    }
}