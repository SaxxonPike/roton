using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Emulation.Kinds;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original, id: 0x04)]
internal sealed class OriginalPlayerKind : IKind
{
    public string FriendlyName => "Player";

    public void Initialize(IElement element)
    {
        element.Character = 0x02;
        element.Color = 0x1F;
        element.IsDestructible = true;
        element.IsPushable = true;
        element.IsAlwaysVisible = true;
        element.Cycle = 1;
        element.MenuIndex = 1;
        element.MenuKey = 'Z';
        element.Name = "Player";
        element.EditorCategory = "Items:";
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}