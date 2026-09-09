using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Emulation.Kinds;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original, id: 0x1D)]
internal sealed class OriginalBlinkWallKind : IKind
{
    public string FriendlyName => "Blink Wall";

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

    public IEnumerable<EditorParam> GetEditorParams() =>
    [
        new("Starting Time", nameof(IActor.P1)),
        new("Period", nameof(IActor.P2)),
        new("Wall Direction", nameof(IActor.Vector), EditorParamType.Vector)
    ];
}