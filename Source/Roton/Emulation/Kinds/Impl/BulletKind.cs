using System.Collections.Generic;
using Roton.Editors;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, id: 0x12)]
[Context(Context.Super, id: 0x45)]
internal sealed class BulletKind : IKind
{
    public string FriendlyName => "Bullet";

    public void Initialize(IElement element)
    {
        element.Character = 0xF8;
        element.Color = 0x0F;
        element.IsDestructible = true;
        element.Cycle = 1;
        element.Name = "Bullet";
    }

    public IEnumerable<EditorParam> GetEditorParams() => [];
}