using Roton.Emulation.Core;

namespace Lyon.Presenters;

/// <summary>
/// Handles the joystick interface to Roton.
/// </summary>
public interface IJoystickPresenter : IJoystick
{
    /// <summary>
    /// Indicates that a joystick has been connected.
    /// </summary>
    void Connect(SDL_JoystickID id);
    
    /// <summary>
    /// Indicates that a joystick has been disconnected.
    /// </summary>
    void Disconnect(SDL_JoystickID id);
    
    /// <summary>
    /// Indicates that a joystick axis has been updated. Values are -1.0 to 1.0.
    /// </summary>
    void UpdateAxis(SDL_JoystickID id, JoystickAxis axis, float value);
    
    /// <summary>
    /// Indicates that a joystick button has been updated.
    /// </summary>
    void UpdateButton(SDL_JoystickID id, JoystickButtons button, bool pressed);
}