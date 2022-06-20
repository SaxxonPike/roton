using DotSDL.Events;
using Roton.Emulation.Core;

namespace Lyon.Presenters;

public interface IKeyboardPresenter : IKeyboard
{
    bool Press(KeyboardEvent data);
}