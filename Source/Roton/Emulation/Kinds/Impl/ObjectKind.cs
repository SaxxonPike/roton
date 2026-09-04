using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x24)]
[Context(Context.Super, 0x24)]
internal sealed class ObjectKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x02;
        element.MenuIndex = 2;
        element.Cycle = 3;
        element.HasDrawCode = true;
        element.MenuKey = 'O';
        element.Name = "Object";
        element.P1EditText = "Character?";
        element.CodeEditText = "Edit Program";
    }
}