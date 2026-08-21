using Roton.Emulation.Core;

namespace Lyon.Presenters;

public interface IKeyboardPresenter : IKeyboard
{
    bool Press(SDL_KeyboardEvent data);
    void Release(SDL_KeyboardEvent data);
}