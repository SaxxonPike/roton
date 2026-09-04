using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Super, 0x3F)]
public class WebKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0xC5;
        element.MenuIndex = 5;
        element.IsEditorFloor = true;
        element.IsFloor = true;
        element.HasDrawCode = true;
        element.MenuKey = 'W';
        element.Name = "Web";
    }
}