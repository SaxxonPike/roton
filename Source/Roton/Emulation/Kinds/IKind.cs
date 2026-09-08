using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;

namespace Roton.Emulation.Kinds;

public interface IKind
{
    string? FriendlyName { get; }

    void Initialize(IElement element);
    IEnumerable<EditorParam> GetEditorParams();
}