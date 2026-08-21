using Roton.Emulation.Core;

namespace Lyon.Presenters;

public interface IJoystickPresenter : IJoystick
{
    void Connect(SDL_JoystickID id);
    void Disconnect(SDL_JoystickID id);
    void UpdateAxis(SDL_JoystickID id, JoystickAxis axis, float value);
    void UpdateButton(SDL_JoystickID id, JoystickButtons button, bool pressed);
}