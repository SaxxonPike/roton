using OpenTK.Input;
using Roton.Emulation.Core;

namespace Roton.Interface.Input
{
    public interface IOpenTkKeyBuffer : IKeyboard
    {
        bool Press(KeyboardKeyEventArgs data);
    }
}