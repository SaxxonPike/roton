using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Kinds.Impl;

[Context(Context.Original, 0x28)]
[Context(Context.Super, 0x28)]
internal sealed class PusherKind : IKind
{
    public void Initialize(IElement element)
    {
        element.Character = 0x10;
        element.HasDrawCode = true;
        element.Cycle = 4;
        element.MenuIndex = 2;
        element.MenuKey = 'P';
        element.Name = "Pusher";
        element.StepEditText = "Push direction?";
    }
}