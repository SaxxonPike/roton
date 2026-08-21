using Roton.Emulation.Core;

namespace Lyon.Presenters;

/// <summary>
/// Handles the keyboard interface to Roton.
/// </summary>
public interface IKeyboardPresenter : IKeyboard
{
    /// <summary>
    /// Indicates that a keyboard key has been pressed.
    /// </summary>
    void Press(SDL_Keycode key, SDL_Keymod mod);

    /// <summary>
    /// Indicates that a keyboard key has been released.
    /// </summary>
    void Release(SDL_Keycode key, SDL_Keymod mod);
}