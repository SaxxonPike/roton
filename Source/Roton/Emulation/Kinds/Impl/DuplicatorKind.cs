using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x0C)]
[Context(Context.Super, 0x0C)]
public class DuplicatorKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xFA;
        element.Color = 0x0F;
        element.Cycle = 2;
        element.HasDrawCode = true;
        element.MenuIndex = 1;
        element.MenuKey = 'U';
        element.Name = "Duplicator";
        element.StepEditText = "Source direction?";
        element.P2EditText = "Duplication rate?;SF";
    }
}