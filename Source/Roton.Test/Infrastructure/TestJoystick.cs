using Roton.Emulation.Core;

namespace Roton.Test.Infrastructure;

public class TestJoystick : IJoystick
{
    public bool IsConnected { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public JoystickButtons Buttons { get; set; }
}