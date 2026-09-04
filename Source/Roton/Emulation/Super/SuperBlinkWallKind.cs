using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Super, 0x1D)]
public class SuperBlinkWallKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xCE;
        element.Cycle = 1;
        element.HasDrawCode = true;
        element.MenuIndex = 3;
        element.MenuKey = 'X';
        element.Name = "Blink wall";
        element.P1EditText = "Starting time";
        element.P2EditText = "Period";
        element.StepEditText = "Wall direction";
    }
}