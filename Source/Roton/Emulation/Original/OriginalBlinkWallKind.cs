using Roton.Emulation.Data;
using Roton.Emulation.Kinds;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original, 0x1D)]
public class OriginalBlinkWallKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xCE;
        element.Cycle = 1;
        element.HasDrawCode = true;
        element.MenuIndex = 3;
        element.MenuKey = 'L';
        element.Name = "Blink wall";
        element.P1EditText = "Starting time";
        element.P2EditText = "Period";
        element.StepEditText = "Wall direction";
    }
}