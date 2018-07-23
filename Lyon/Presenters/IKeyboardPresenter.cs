using OpenTK.Input;
using Roton.Emulation.Core;

namespace Lyon.Presenters
{
    public interface IKeyboardPresenter : IKeyboard
    {
        bool Press(KeyboardKeyEventArgs data);
    }
}