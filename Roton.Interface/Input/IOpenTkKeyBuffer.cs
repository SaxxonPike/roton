using OpenTK.Input;
using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Interface.Input
{
    public interface IOpenTkKeyBuffer : IKeyboard
    {
        bool Press(KeyboardKeyEventArgs data);
    }
}