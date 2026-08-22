using System;

namespace Roton.Emulation.Core;

[Flags]
public enum JoystickButtons
{
    Ok = 1 << 0,
    Cancel = 1 << 1,
    Left = 1 << 2,
    Right = 1 << 3,
    Up = 1 << 4,
    Down = 1 << 5,
    PageUp = 1 << 6,
    PageDown = 1 << 7,
    Shoot = 1 << 8,
    Start = 1 << 9
}