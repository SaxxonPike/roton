namespace Roton.Emulation.Core;

public interface IJoystick
{
    bool IsConnected { get; }
    float X { get; }
    float Y { get; }
    JoystickButtons Buttons { get; }
}