using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Emulation.Kinds;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original, id: 0x03)]
internal sealed class OriginalMonitorKind : IKind
{
    public string FriendlyName => "Monitor";

    public void Initialize(IElement element)
    {
        element.Character = 0x20;
        element.Color = 0x07;
        element.Cycle = 1;
        element.Name = "Monitor";
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}